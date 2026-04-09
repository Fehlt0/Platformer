using System;
using UnityEngine;

public class CameraChange : MonoBehaviour
{
    [SerializeField] private int id;
    private void OnTriggerEnter2D(Collider2D other)
    {
        CameraManager.instance.SetNewTarget(id);
    }
}
