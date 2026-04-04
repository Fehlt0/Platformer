using System;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] private int id;

    private void Start()
    {
        transform.SetParent(null);
        DontDestroyOnLoad(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && GameManager.instance.lastCheckpoint < id)
        {
            Debug.Log("détecté");
            GameManager.instance.lastCheckpoint = id;
        }
    }
}
