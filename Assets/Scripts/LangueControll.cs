using System;
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
    
    private bool isGrappling;

    private void FixedUpdate()
    {
        if (isGrappling)
        {
            GrappleMove();
            rb.linearVelocity = Vector2.zero;
            
            tongueLine.enabled = true;
            tongueLine.SetPosition(0, transform.position);
            tongueLine.SetPosition(1, grapplePoint); 
        }
        else
        {
            tongueLine.enabled = false;
        }
    } 
    
    public void OnTongue(InputAction.CallbackContext context)
    {
        if (context.started)
        {

            TryGrapple();
        }

        if (context.canceled)
        {
            isGrappling = false;
        }
    }

    private void TryGrapple()
    {
        if (aimInput.magnitude < 0.2f)
        {
            Debug.Log(aimInput);
            return;
        }
        
        Vector2 direction = aimInput.normalized;
        
        
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, tongueDistance, grappleLayer);
        
        //Debug.DrawRay(transform.position, direction, Color.red);
        Debug.Log(hit.collider);
        
        if (hit.collider != null)
        {
            grapplePoint = hit.point;
            isGrappling = true;
        }
    }
    
    private void GrappleMove()
    {
        Vector2 direction = (grapplePoint - (Vector2)transform.position).normalized;

        rb.linearVelocity = direction * tonguePullForce;
        
        
        float distance = Vector2.Distance(transform.position, grapplePoint);
        if (distance < 0.5f)
        {
            isGrappling = false;
            rb.linearVelocity = Vector2.zero;
        }
    }
    
    public void OnAim(InputAction.CallbackContext context)
    {
        
        aimInput = context.ReadValue<Vector2>();

    }
}
