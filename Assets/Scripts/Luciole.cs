using System;
using UnityEngine;

public class Luciole : MonoBehaviour
{
    public float speed;
    public float bounceForce;
    
    public bool hasBounce = false;

    public Vector2 direction;
    public GameObject plant;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction.normalized * speed;
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("plant"))
        {
            StickToTarget(collision);
            
        }
        else
        {
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
    }

    private void StickToTarget(Collision2D collision)
    {
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;

        transform.parent = collision.transform;
    }
    
    private void Bounce(Collision2D collision)
    {
        Vector2 normal  = collision.contacts[0].normal;
        Vector2 direction = Vector2.Reflect(rb.linearVelocity.normalized, normal);
        
        rb.linearVelocity = direction * bounceForce;
    }

    private void StopProjectile()
    {
     rb.linearVelocity = Vector2.zero;
     rb.bodyType = RigidbodyType2D.Static;
    }

    
}
