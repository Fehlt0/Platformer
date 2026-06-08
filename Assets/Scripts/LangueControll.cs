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


    [SerializeField] public LineRenderer tongueLine;
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
    
    public bool tongueGoing;
    private bool hasLatched;
    
    private float tongueProgress;
    private float minDistanceBeforeAJ = 0.5f ; // AJ = AutoJump 0.15 valeur default 0.75 ou 1
                                               // pour plus de punch genre ça snap un peu plus
    
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
        else if(!tongueGoing)
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

                currentTarget?.SetSpriteHanging(true);
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
        
        currentTarget.SetSpriteOnTarget(false);
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
        
        currentTarget?.SetSpriteOnTarget(false);
        currentTarget = nearest;
        currentTarget?.SetSpriteOnTarget(true);
    }

    private void GrappleMove()
    {
        if (LanguePlant.listPlanteLangue.Count == 0)
        {
            return;
        }



        LanguePlant nearest = GetNearestPlant(out float minDistance);
        if (nearest == null)
        {
            return;
        }

        if (minDistance > tongueDistance)
        {
            return;
        }

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

        if (minDistance < minDistanceBeforeAJ)
        {
            transform.position = grapplePoint;

            isGrappling = false;
            tongueLine.enabled = false;
            
            
            playerController.TriggerTongueJump();
            
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
            UpdateTarget();
            if (currentTarget == null)
            {
                return;
            }
            
            wasHoldingTongue = false;
            hasLatched = false;
            isGrappling = true;
            tongueProgress = 0f;
            tongueGoing = true;
            tongueLine.enabled = true;
        }

        if (context.canceled)
        {
            currentTarget?.SetSpriteHanging(false);
            
            StopGrapple();
            hasLatched = false;
            animatorRef.SetBool("isOnTongue", false);
            if (tongueTip != null)
            {
                tongueTip.gameObject.SetActive(false);
            }
        }
    }
}
