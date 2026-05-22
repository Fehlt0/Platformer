using System;
using UnityEngine;

public class CameraChange : MonoBehaviour
{
    [SerializeField] private int id;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CameraManager.instance.SetNewTarget(id);
        }
    }
}
