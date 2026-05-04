using System;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] private int idCam;

    private void Start()
    {
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && GameManager.instance.lastCheckpoint < id)
        {
            GameManager.instance.lastCheckpoint = id;
            GameManager.instance.lastCamTarget = idCam;
        }
    }
}
