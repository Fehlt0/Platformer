using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LangueControll : MonoBehaviour
{
    [SerializeField] private float tongueDistance = 10f;
    [SerializeField] private float tonguePullForce = 20f;
    
    [SerializeField] private LayerMask grappleLayer;
    
    [SerializeField] private LineRenderer tongueLine;

    private Rigidbody2D rb;
    
    private Vector2 grapplePoint;
    private Vector2 aimInput;
    private Vector2 direction;
    
    private LanguePlant currentTarget;
    
    public bool wasHoldingTongue;
    public bool isGrappling;

    
   
    

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Update()
    {
        UpdateTarget();
    }

    /*private void Update()
    {
       ShowAim();
    }*/

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
            wasHoldingTongue =  false;
            isGrappling = true; 

        }

        if (context.canceled)
        {
            wasHoldingTongue = false;
            isGrappling = false;
        }
    }
    
    
    private void UpdateTarget()
    {
        if (LanguePlant.listPlanteLangue.Count == 0) return;

        LanguePlant nearest = null;
        float minDistance = Mathf.Infinity;

        foreach (var plante in LanguePlant.listPlanteLangue)
        {
            if (plante == null) continue;

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

        wasHoldingTongue = true;
        
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
        

        if (minDistance <= tongueDistance)
        {
            Vector2 direction = (grapplePoint - (Vector2)transform.position).normalized;
            
            rb.linearVelocity = direction * tonguePullForce;
            
            wasHoldingTongue = true;
        }

        if (minDistance < 0.01f)
        {
            isGrappling = false;
            rb.linearVelocity = Vector2.zero;
        }
    }

    
    /*private void GrappleMove()
    {
        
        var langueGrapple = LanguePlant.listPlanteLangue[0];
        
        foreach (var plante in LanguePlant.listPlanteLangue)
        {
            if (Vector2.Distance(plante.transform.position, transform.position) <=
                Vector2.Distance(langueGrapple.transform.position, transform.position))
            {
                langueGrapple = plante;
            }
        }

        if (Vector2.Distance(langueGrapple.transform.position, transform.position) <= tongueDistance )
        {
            Vector2 direction = (langueGrapple.transform.position -  transform.position).normalized;
            rb.linearVelocity = direction * tonguePullForce;
            wasHoldingTongue = true;

        }
        float distance = Vector2.Distance(transform.position, grapplePoint);
        if (distance < 0.01f )
        {
            isGrappling = false;
            rb.linearVelocity = Vector2.zero;
        }
        
    }*/
    
    /*private void TryGrapple()
    {
        if (aimInput.magnitude < 0.2f)
        {
            Debug.Log("Aim: " + aimInput);
            return;
        }

        Vector2 direction = aimInput.normalized;

        Debug.DrawRay(transform.position, direction * tongueDistance, Color.red, 1f);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, tongueDistance, grappleLayer);

        Debug.Log(hit.collider);


        if (hit.collider != null)
        {
            grapplePoint = hit.point;
            isGrappling = true;
        }
    }*/
    
    /*private void GrappleMove()
    {
        Vector2 direction = (grapplePoint - (Vector2)transform.position).normalized;

        rb.linearVelocity = direction * tonguePullForce;
        
        float distance = Vector2.Distance(transform.position, grapplePoint);
        if (distance < 0.5f)
        {
            isGrappling = false;
            rb.linearVelocity = Vector2.zero;
        }
    }*/
    
    /*public void OnAim(InputAction.CallbackContext context)
    {

        aimInput = context.ReadValue<Vector2>();


    }*/

    /*private void ShowAim()
    {
        if ( !isHoldingTongue || aimInput.magnitude < 0.2f)
        {
            tongueLine.enabled = false;
            return;
        }

        Vector2 direction = aimInput.normalized;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, tongueDistance, grappleLayer);

        tongueLine.enabled = true;
        tongueLine.SetPosition(0, transform.position);

        if (hit.collider != null)
        {
            tongueLine.material.color = Color.green;
            tongueLine.SetPosition(1, hit.point);
        }
        else
        {
            tongueLine.material.color = Color.red;
            tongueLine.SetPosition(1, (Vector2)transform.position + direction * tongueDistance);
        }
    }*/
}
