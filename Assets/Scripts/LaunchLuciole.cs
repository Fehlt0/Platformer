using UnityEngine;
using UnityEngine.InputSystem;

public class LaunchLuciole : MonoBehaviour
{

    public GameObject luciolePrefab;
    public Transform firePoint;
    public float launchSpeed = 10f;

    private Vector2 aimDirection;
    
    private GameObject currentLuciole;


    public void Update()
    {
        aimDirection = Gamepad.current.rightStick.ReadValue();
    }


    public void OnLaunch(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Launch();
        }
    }

    private void Launch()
    {

        if (aimDirection.sqrMagnitude < 0.1f)
        {
            Debug.Log("Launching Luciole");
            return;
        }


        Vector2 direction = aimDirection.normalized;
        
        if (currentLuciole != null)
        {
            Destroy(currentLuciole);
        }

        currentLuciole = Instantiate(luciolePrefab, firePoint.position, Quaternion.identity);

        
        Rigidbody2D rb = currentLuciole.GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction * launchSpeed;


        Luciole script = currentLuciole.GetComponent<Luciole>();
        if (script != null)
        {
            script.direction = direction;
        }
    }
}
