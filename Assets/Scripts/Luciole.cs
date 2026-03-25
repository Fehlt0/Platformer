using System;
using UnityEngine;

public class Luciole : MonoBehaviour
{
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (rb.linearVelocity == Vector2.zero)
        {
            Arret();
        }
    }

    private void Arret()
    {
        Debug.Log("arrêt");
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("plant"))
        {
            Arret();
        }
    }
}
