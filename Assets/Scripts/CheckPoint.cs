using System;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] private int id;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.lastCheckpoint = id;
        }
    }
}
