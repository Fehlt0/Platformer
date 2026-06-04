using System;
using UnityEngine;

public class CameraChange : MonoBehaviour
{
    private BoxCollider2D collider2D;

    private void Start()
    {
        collider2D = GetComponent<BoxCollider2D>();
    }

    [SerializeField] private int id;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CameraManager.instance.SetNewTarget(id);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            collider2D.isTrigger = false;
        }
    }
}
