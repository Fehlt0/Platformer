using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LangueControll : MonoBehaviour
{
    
    [SerializeField] private float tongueDistance = 10f;
    [SerializeField] private float tonguePullForce = 20f;
    [SerializeField] private float tongueSpeed = 15f;


    [SerializeField] private LayerMask grappleLayer;
    [SerializeField] private LayerMask obstacleLayer;


    [SerializeField] private LineRenderer tongueLine;
    [SerializeField] private Transform mouthPoint;
    [SerializeField] private Transform tongueTip;
    public GameObject playerHead;
    [SerializeField] private GameObject playerArm;

    public Animator animatorRef;

    private Rigidbody2D rb;
    private PlayerController playerController;
    private SpriteRenderer headSpriteRenderer;

    private Vector2 grapplePoint;
    private LanguePlant currentTarget;

    public bool wasHoldingTongue;
    public bool isGrappling;
    private bool tongueGoing;
    private bool hasLatched;
    private float tongueProgress;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animatorRef = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
        headSpriteRenderer = playerHead.GetComponentInChildren<SpriteRenderer>();

        InitTongueLine();
    }

    private void InitTongueLine()
    {
        tongueLine.positionCount = 2;
        
        tongueLine.startWidth = 0.25f;
        tongueLine.endWidth = 0.18f;
        
        tongueLine.numCapVertices = 10;
        tongueLine.material = new Material(Shader.Find("Sprites/Default"));
        
        tongueLine.startColor = new Color(0.9f, 0.1f, 0.1f);
        tongueLine.endColor = new Color(0.6f, 0f, 0f);
        
        tongueLine.sortingLayerName = "Default";
        tongueLine.sortingOrder = -2;
        
        tongueLine.enabled = false;
    }

    private void Update()
    {
        UpdateTarget();
        UpdateTongueLine();
    }

    private void FixedUpdate()
    {
        if (isGrappling)
        {
            GrappleMove();
        }
        else
        {
            ClearTarget();
        }
    }

    private void UpdateTongueLine()
    {
        if (!tongueLine.enabled || currentTarget == null)
        {
            return;
        }

        grapplePoint = currentTarget.transform.position;

        if (tongueGoing)
        {
            tongueProgress += Time.deltaTime * tongueSpeed;
            
            if (tongueProgress >= 1f)
            {
                tongueProgress = 1f;
                tongueGoing = false;
                hasLatched = true;
            }
        }

        Vector2 start = mouthPoint.position;
        Vector2 end = grapplePoint;
        
        float t = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(tongueProgress));
        Vector2 currentPoint = Vector2.Lerp(start, end, t);

        tongueLine.SetPosition(0, start);
        tongueLine.SetPosition(1, Vector2.Distance(start, end) < 0.5f ? start : currentPoint);

        if (tongueTip != null)
        {
            tongueTip.position = tongueLine.GetPosition(1);
            tongueTip.gameObject.SetActive(true);
        }
    }

    private void ClearTarget()
    {
        if (currentTarget == null)
        {
            return;
        }
        
        currentTarget.SetColorOnTarget(false);
        currentTarget = null;
    }

    private void UpdateTarget()
    {
        if (LanguePlant.listPlanteLangue.Count == 0)
        {
            return;
        }

        LanguePlant nearest = null;
        float minDistance = Mathf.Infinity;

        foreach (var plante in LanguePlant.listPlanteLangue)
        {
            if (plante == null)
            {
                continue;
            }
            float dist = Vector2.Distance(plante.transform.position, transform.position);
            
            if (dist <= tongueDistance && dist < minDistance)
            {
                minDistance = dist;
                nearest = plante;
            }
        }

        if (currentTarget == nearest)
        {
            return;
        }
        
        currentTarget?.SetColorOnTarget(false);
        currentTarget = nearest;
        currentTarget?.SetColorOnTarget(true);
    }

    private void GrappleMove()
    {
        if (LanguePlant.listPlanteLangue.Count == 0)
        {
            return;
        }

        LanguePlant nearest = GetNearestPlant(out float minDistance);
        if (nearest == null) return;

        grapplePoint = nearest.transform.position;
        AnimLangue();

        Vector2 origin = transform.position;
        Vector2 dir = (grapplePoint - origin).normalized;
        float distance = Vector2.Distance(origin, grapplePoint);

        if (Physics2D.Raycast(origin, dir, distance, obstacleLayer).collider != null)
        {
            isGrappling = false;
            return;
        }

        Debug.DrawRay(origin, dir * distance, Color.red);

        if (minDistance <= tongueDistance)
        {
            wasHoldingTongue = true;
            rb.linearVelocity = dir * tonguePullForce;
        }
        else
        {
            StopGrapple();
            return;
        }

        if (minDistance < 0.15f)
        {
            transform.position = grapplePoint;
            rb.linearVelocity = Vector2.zero;
        }
    }

    private LanguePlant GetNearestPlant(out float minDistance)
    {
        LanguePlant nearest = null;
        minDistance = Mathf.Infinity;

        foreach (var plante in LanguePlant.listPlanteLangue)
        {
            if (plante == null)
            {
                Debug.LogWarning("Plant not available");
                continue;
            }
            
            float dist = Vector2.Distance(plante.transform.position, transform.position);
            
            if (dist < minDistance)
            {
                minDistance = dist;
                nearest = plante;
            }
        }
        return nearest;
    }

    private void AnimLangue()
    {
        animatorRef.SetBool("isOnTongue", true);
        headSpriteRenderer.flipX = !playerController.facingRight;
    }

    private void StopGrapple()
    {
        isGrappling = false;
        wasHoldingTongue = false;
        tongueLine.enabled = false;
    }

    public void OnTongue(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            wasHoldingTongue = false;
            hasLatched = false;
            isGrappling = true;
            tongueProgress = 0f;
            tongueGoing = true;
            tongueLine.enabled = true;
        }

        if (context.canceled)
        {
            StopGrapple();
            hasLatched = false;
            animatorRef.SetBool("isOnTongue", false);
            if (tongueTip != null)
            {
                tongueTip.gameObject.SetActive(false);
            }
        }
    }
    
    
    
    
    /*
    [SerializeField] private float tongueDistance = 10f;
    [SerializeField] private float tonguePullForce = 20f;
    
    [SerializeField] private LayerMask grappleLayer;
    [SerializeField] private LayerMask obstacleLayer;
    
    [SerializeField] private LineRenderer tongueLine;
    [SerializeField] private Transform mouthPoint;
    
    public Animator animatorRef;

    private Rigidbody2D rb;
    
    private Vector2 grapplePoint;
    private Vector2 aimInput;
    private Vector2 direction;
    
    private LanguePlant currentTarget;
    private LanguePlant lockedTarget;
    
    public bool wasHoldingTongue;
    public bool isGrappling;
    private bool tongueGoing;
    private bool hasLatched;
    
    
    [SerializeField] private float tongueSpeed = 15f;
    [SerializeField] private Transform tongueTip;
    
    
    public GameObject playerHead;
    [SerializeField] private GameObject playerArm;
    
    private float tongueProgress;
    


    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animatorRef = GetComponent<Animator>();
        
        tongueLine.positionCount = 2;

        tongueLine.startWidth = 0.25f;
        tongueLine.endWidth = 0.18f;

        tongueLine.numCapVertices = 10; 
        tongueLine.material = new Material(Shader.Find("Sprites/Default"));

        tongueLine.startColor = new Color(0.9f, 0.1f, 0.1f);
        tongueLine.endColor = new Color(0.6f, 0f, 0f);

        tongueLine.enabled = false;
        
        tongueLine.sortingLayerName = "Default"; 
        tongueLine.sortingOrder = -2;
        
        
    }

    public void Update()
    {
        UpdateTarget();
        
        if (tongueLine.enabled && currentTarget != null)
        {
            grapplePoint = currentTarget.transform.position;

            if (tongueGoing)
            {
                tongueProgress += Time.deltaTime * tongueSpeed;
            }

            if (tongueProgress >= 1f && tongueGoing)
            {
                tongueGoing = false;
                hasLatched = true;
            }
                

            tongueProgress = Mathf.Clamp01(tongueProgress);

            Vector2 start = mouthPoint.position;
            Vector2 end = grapplePoint;


            float t = Mathf.SmoothStep(0f, 1f, tongueProgress);
            Vector2 currentPoint = Vector2.Lerp(start, end, t);



            tongueLine.SetPosition(0, start);
            tongueLine.SetPosition(1, currentPoint);
            
            float distToTarget = Vector2.Distance(start, end);

            if (distToTarget < 0.5f)
            {
                tongueLine.SetPosition(1, start);
            }
            
            if (tongueTip != null)
            {
                tongueTip.position = tongueLine.GetPosition(1);
                tongueTip.gameObject.SetActive(tongueLine.enabled);
            }
        }
        

    }


    private void FixedUpdate()
    {
        if (isGrappling)
        {
            GrappleMove();
        }
        else
        {
            if (currentTarget != null)
            {
                currentTarget.SetColorOnTarget(false);
                currentTarget = null;
            }
        }
        
    } 
    
    public void OnTongue(InputAction.CallbackContext context)
    {
        
        
        if (context.performed)
        {

           
           wasHoldingTongue = false;
           hasLatched = false; 
           isGrappling = true;
           tongueProgress = 0f;
           tongueGoing = true;
           tongueLine.enabled = true;
           
           
           


        }

        if (context.canceled)
        {

            
            wasHoldingTongue = false;
            isGrappling = false;
            hasLatched = false;
            tongueLine.enabled = false;
            animatorRef.SetBool("isOnTongue", false);

        }
    }
    
    
    private void UpdateTarget()
    {

        if (LanguePlant.listPlanteLangue.Count == 0)
        {
            return;
        }

        LanguePlant nearest = null;
        float minDistance = Mathf.Infinity;

        foreach (var plante in LanguePlant.listPlanteLangue)
        {
            if (plante == null)
            {
                continue;
            }

            float dist = Vector2.Distance(plante.transform.position, transform.position);

            
            if (dist <= tongueDistance && dist < minDistance)
            {
                minDistance = dist;
                nearest = plante;
            }
        }

        
        if (currentTarget != nearest)
        {
            currentTarget?.SetColorOnTarget(false);

            currentTarget = nearest;

            currentTarget?.SetColorOnTarget(true);
        }
    }

    

    
    private void GrappleMove()
    {
        if (LanguePlant.listPlanteLangue.Count == 0) return;
        
        LanguePlant langueGrapple = null;
        float minDistance = Mathf.Infinity;

        foreach (var plante in LanguePlant.listPlanteLangue)
        {

            if (plante == null )
            {
                Debug.LogWarning("not available");
                continue;
            }

            float dist = Vector2.Distance(plante.transform.position, transform.position);

            
            if (dist < minDistance)
            {

                minDistance = dist;
                langueGrapple = plante;
                
            }
        }

        if (langueGrapple == null)
        {
            Debug.LogWarning("langueGrapple is null");
            return;
        }
        
        grapplePoint = langueGrapple.transform.position;
        AnimLangue(grapplePoint);
        Vector2 origin = transform.position;
        Vector2 dir = (grapplePoint - origin).normalized;
        float distance = Vector2.Distance(origin, grapplePoint);

        RaycastHit2D hit = Physics2D.Raycast(origin, dir, distance, obstacleLayer);
        
        if (hit.collider != null)
        {
            Debug.Log("Mur détecté !");
            isGrappling = false;
            return;

            
        }
        
        Debug.DrawRay(origin, dir * distance, Color.red);
        
        

        if (minDistance <= tongueDistance )
        {
            wasHoldingTongue = true;
            Vector2 direction = (grapplePoint - (Vector2)transform.position).normalized;

            
            //rb.linearVelocity = direction * tonguePullForce;
            rb.linearVelocity = new Vector2(direction.x * tonguePullForce, direction.y * tonguePullForce);
            
        }
        else
        {
            isGrappling = false;
            wasHoldingTongue = false;
            tongueLine.enabled = false;
            return;
        }
        

        if (minDistance < 0.15f)
        {
            transform.position = grapplePoint;
            rb.linearVelocity =  Vector2.zero;

            
        }
    }


   
   private void AnimLangue(Vector2 grapplePoint)
   {
       animatorRef.SetBool("isOnTongue", true);

       PlayerController playerController = GetComponent<PlayerController>();
       SpriteRenderer teteSR = playerHead.GetComponentInChildren<SpriteRenderer>();
       teteSR.flipX = !playerController.facingRight;
   }*/
    

    

    
}
