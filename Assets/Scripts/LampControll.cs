using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class LampControll : MonoBehaviour
{
    private GameObject player;
    private Vector3 decalage;
    [SerializeField] private float distance;
    private void Start()
    {
        player = GameObject.Find("Player");
        Vector2 joystick = Gamepad.current.rightStick.ReadValue();
        joystick.Normalize();
        decalage = new Vector3(joystick.x, joystick.y, 0f) * distance;
        Destroy(gameObject, PlayerController.lampTimer);
    }

    private void Update()
    {
        transform.position = player.transform.position + decalage;
        
    }
}
