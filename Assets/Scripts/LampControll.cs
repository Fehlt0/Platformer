using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class LampControll : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    
    private GameObject player;
    private Vector3 decalage;
    [SerializeField] private float distance;
    private void Start()
    {
        player = GameObject.Find("Player(Clone)");
        Vector2 joystick = Gamepad.current.rightStick.ReadValue();
        joystick.Normalize();
        decalage = new Vector3(joystick.x, joystick.y, 0f) * distance;
        Destroy(gameObject, playerData.lampTimer);
    }

    private void Update()
    {
        Debug.Log(player);
        Debug.Log(player.transform.position);
        Debug.Log(decalage);
        transform.position = player.transform.position + decalage;
    }
}
