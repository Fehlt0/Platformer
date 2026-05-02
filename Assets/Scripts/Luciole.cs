using System;
using UnityEngine;

public class Luciole : MonoBehaviour
{
    public float speed;
    public float bounceForce;
    
    public bool hasBounce = false;

    public Vector2 direction;

    public Plant currentPlant;


    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction.normalized * speed;
        
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("plant"))
        {
            StickToTarget(other.transform);
            
        }
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (currentPlant != null)
        {
            currentPlant.SetHit(false);
            currentPlant = null;
        }
        
        if (hasBounce)
        {
            StopProjectile();
        }
        else
        {
            Bounce(collision);
            hasBounce = true;
        }
        
    }

    private void StickToTarget(Transform target)
    {
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;

        transform.parent = target;
        
        currentPlant = target.GetComponent<Plant>();

        if (currentPlant != null)
        {
            currentPlant.SetHit(true);
        }
    }
    private void Bounce(Collision2D collision)
    {
        Vector2 normal = collision.contacts[0].normal;

        Vector2 reflected = Vector2.Reflect(rb.linearVelocity, normal);

        rb.linearVelocity = reflected;
    }

    private void StopProjectile()
    {
        if (currentPlant != null)
        {
            currentPlant.SetHit(false);
            currentPlant = null;
        }
        
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static;
    }
    
    private void OnDestroy()
    {
        if (currentPlant != null)
        {
            currentPlant.SetHit(false);
            currentPlant = null;
        }
    }

    
}
