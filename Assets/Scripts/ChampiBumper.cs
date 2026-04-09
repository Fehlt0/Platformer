using System;
using UnityEngine;

public class ChampiBumper : Plant
{
    [SerializeField] private float strength;
    private Vector2 direction;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (isAlive)
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
        
            other.rigidbody.AddForce(direction * strength);
        }
    }
}
