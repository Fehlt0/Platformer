using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class LaunchLuciole : MonoBehaviour
{

    public GameObject luciolePrefab;
    public Transform firePoint;
    public float launchSpeed = 10f;
    public float returnSpeed = 20f;

    private Vector2 aimDirection;
    
    private GameObject currentLuciole;
    private bool lucioleAlreadyLaunched = false;
    



    public void Update()
    {
        aimDirection = Gamepad.current.rightStick.ReadValue();
    }


    public void OnLaunch(InputAction.CallbackContext context)
    {
        if (context.performed && !lucioleAlreadyLaunched)
        {

            lucioleAlreadyLaunched = true;
            Launch();
            
            
        }
        else if (context.performed && lucioleAlreadyLaunched)
        {

            lucioleAlreadyLaunched = false;
            GoBack();
            
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
        
        currentLuciole = Instantiate(luciolePrefab, firePoint.position, Quaternion.identity);

        // pour ta luciole, j'aurais surement mis tous ces composants ( rb2d, CircleCollider, etc ) directement accessible dedans
        // pour te permettre d'y accéder sans les get component, et ici je serais parti pour changer le type de currentLuciole en Luciole directement
        // comme ca tu l'instancies, tu get component luciole, et t'as accès à tout
        
        Rigidbody2D rb = currentLuciole.GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction * launchSpeed;


        Luciole script = currentLuciole.GetComponent<Luciole>();
        if (script != null)
        {
            script.direction = direction;
        }
    }
    private void GoBack()
    {
        if (currentLuciole == null) return;

        Rigidbody2D rb = currentLuciole.GetComponent<Rigidbody2D>();
        Luciole script = currentLuciole.GetComponent<Luciole>();
        CircleCollider2D collider = currentLuciole.GetComponent<CircleCollider2D>();

        rb.bodyType = RigidbodyType2D.Dynamic;

        if (script != null)
        {
            script.enabled = false;
        }

        if (collider != null)
        {
            collider.enabled = false;
        }

        StartCoroutine(ReturnToPlayer(rb));
    }

    private IEnumerator ReturnToPlayer(Rigidbody2D rb)
    {
        while (currentLuciole != null)
        {
            Vector2 directionToPlayer = firePoint.position - currentLuciole.transform.position;

            if (directionToPlayer.magnitude < 0.2f)
            {
                Destroy(currentLuciole);
                currentLuciole = null;
                break;
            }

            rb.linearVelocity = directionToPlayer.normalized * returnSpeed;

            yield return null; 
        }
    }
}
