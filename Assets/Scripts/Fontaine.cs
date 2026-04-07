using System;
using UnityEngine;

public class Fontaine : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    private float maxDryCount;

    private void Start()
    {
        maxDryCount = playerData.dryCount;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().dryCount = maxDryCount;
        }
    }
}
