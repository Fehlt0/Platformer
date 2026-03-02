using System;
using UnityEngine;

public class Plant : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Light"))
        {
            Debug.Log("détecté");
        }
    }
}
