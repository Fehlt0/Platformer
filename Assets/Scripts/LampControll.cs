using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class LampControll : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;

    private Transform playerTransform;
    private Vector3 decalage;
    [SerializeField] private float distance;
    private void Start()
    {
        playerTransform = GetComponent<Transform>();
        Vector2 joystick = Gamepad.current.rightStick.ReadValue();
        joystick.Normalize();
        decalage = new Vector3(joystick.x, joystick.y, 0f) * distance;
        Destroy(gameObject, playerData.lampTimer);
    }

    private void Update()
    {
        transform.position = playerTransform.position + decalage;
    }
}
