using System;
using UnityEngine;

public class Spike : MonoBehaviour
{
    private PlayerController player;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().Die();
        }
    }
}
