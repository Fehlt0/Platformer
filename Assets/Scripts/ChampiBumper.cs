using System;
using UnityEngine;

public class ChampiBumper : Plant
{
    [SerializeField] private float strength;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (isAlive)
        {
            Debug.Log("collision");
            Vector3 a = transform.position;
            Vector3 b = other.transform.position;
            Vector3 direction;
            direction = b - a;
            direction = direction.normalized;
        
            other.rigidbody.AddForce(direction * strength);
        }
    }
}
