using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class LaunchLuciole : MonoBehaviour
{

    public GameObject luciolePrefab;
    public Transform firePoint;
    
    
    public Animator animatorRef;
    
    public SpriteRenderer RefpropulsionLuciole;
    
    public float launchSpeed = 10f;
    public float returnSpeed = 20f;

    private SpriteRenderer sprite;
    
    [SerializeField] private GameObject pointeur;

    private Vector2 aimDirection;
    
    private GameObject currentLuciole;
    private bool lucioleAlreadyLaunched = false;

    public void Awake()
    {
        sprite = currentLuciole.GetComponent<SpriteRenderer>();
    }


    public void Update()
    {
        aimDirection = Gamepad.current.rightStick.ReadValue();

        if (gameObject == null)
        {
            Destroy(currentLuciole);
        }
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
        
        sprite.color = Color.yellow;

        Vector2 direction = aimDirection.normalized;
        
        currentLuciole = Instantiate(luciolePrefab, firePoint.position, Quaternion.identity);

        
        Rigidbody2D rb = currentLuciole.GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction * launchSpeed;


        Luciole script = currentLuciole.GetComponent<Luciole>();
        if (script != null)
        {
            script.direction = direction;
        }
        AnimLucioleLaunch();
    }
    private void GoBack()
    {
        if (currentLuciole == null) return;

        Rigidbody2D rb = currentLuciole.GetComponent<Rigidbody2D>();
        Luciole script = currentLuciole.GetComponent<Luciole>();
        CircleCollider2D collider = currentLuciole.GetComponent<CircleCollider2D>();
        
        
        sprite.color = Color.darkRed;

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
    
    private void AnimLucioleLaunch()
    {
        //animatorRef.SetBool("isOnTongue", true);
        RefpropulsionLuciole.transform.rotation = pointeur.transform.rotation;
    }

}
