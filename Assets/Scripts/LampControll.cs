using UnityEngine;
using UnityEngine.InputSystem;

public class LampControll : MonoBehaviour
{
    public Transform player;

    public float distance = 2f;
    
    void Update()
    {

        Vector2 joystick = Gamepad.current.rightStick.ReadValue();

        if (joystick.magnitude > 0.1f)
        {
            joystick.Normalize();
            
            Vector3 decalage = new Vector3(joystick.x, joystick.y, 0f) * distance ;
            transform.position = player.position + decalage;
            
            float angle = Mathf.Atan2(decalage.y, decalage.x) * Mathf.Rad2Deg; 
            transform.rotation = Quaternion.Euler(0f, 0f, angle +90f);
        }
    }
}
