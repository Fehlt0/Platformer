using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class ChampiBumper : Plant
{
    [SerializeField] private float strength;
    private Vector2 direction;
    
    public override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        if (isAlive && other.CompareTag("Player"))
        {
            float rotation = transform.eulerAngles.z;
            rotation *= Mathf.Deg2Rad;

            Vector2 direction = new Vector2(-Mathf.Sin(rotation), Mathf.Cos(rotation));

            other.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            other.GetComponent<Rigidbody2D>().AddForce(direction * strength);
        }
    }
}
