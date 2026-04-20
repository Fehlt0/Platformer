using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class ChampiBumper : Plant
{
    [SerializeField] private float strength;
    private Vector2 direction;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isAlive && other.CompareTag("Player"))
        {
            //Debug.Log(transform.eulerAngles.z);
            switch (transform.eulerAngles.z)
            {
                case 0:
                    direction =  new Vector2(0,1);
                    break;
                case >= 270:
                    direction = new Vector2(1,1);
                    break;
                case <= 90:
                    direction = new Vector2(-1, 1);
                    break;
            }

            other.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            other.GetComponent<Rigidbody2D>().AddForce(direction * strength);
        }
    }
}
