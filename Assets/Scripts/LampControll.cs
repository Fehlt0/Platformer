using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class LampControll : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject, PlayerController.lampTimer);
    }
}
