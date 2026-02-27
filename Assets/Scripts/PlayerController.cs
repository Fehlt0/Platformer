using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    
    [SerializeField] float currentSpeed = 1f;
    
    
    [SerializeField] float jumpForce= 10f;
    [SerializeField] float distanceOfGroundJump= 1f;
    [SerializeField] private float coyoteTime = 0.2f;
    [SerializeField] private float jumpBufferTime = 0.2f;
    [SerializeField] private float airTimeKill = 1f;
    
    
    private Vector2 moveInput;
    private Transform cameraTransform;
    private Rigidbody2D rb;
    private float coyoteTimeCounter;
    private float jumpBufferTimeCounter;
    private float lastJump;
    private float airTime;
    
    private bool isGrounded;
    private bool isWall;

    
    
    public LayerMask layerMask;
    
    
   
    void Start()
    {
        cameraTransform = Camera.main.transform;
        rb = GetComponent<Rigidbody2D>();
        currentCooldownJump = initCooldownJump;

    }

    void Update()
    {
        if (isGrounded)
        {
            if (airTime >= airTimeKill)
            {
                Debug.Log("mort sale nul");
            }
            coyoteTimeCounter = coyoteTime;
            airTime = 0;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
            airTime += Time.deltaTime;
        }

        jumpBufferTimeCounter -= Time.deltaTime;
        
        if (jumpBufferTimeCounter > 0f && coyoteTimeCounter > 0f && Time.time -lastJump > 0.5f)
        {
            rb.AddForce(jumpForce *  Vector2.up, ForceMode2D.Impulse);
            lastJump = Time.time;
            coyoteTimeCounter = 0f;
            jumpBufferTimeCounter = 0f;
        }
    }

    private void FixedUpdate()
    {
        CheckGround();
        
        rb.linearVelocity = new Vector2(moveInput.x * currentSpeed, rb.linearVelocity.y);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            jumpBufferTimeCounter = jumpBufferTime;
        }
    }
    


    

    private void CheckGround()
    {
        RaycastHit2D hitGround = Physics2D.Raycast(transform.position, Vector2.down, distanceOfGroundJump, layerMask);
        isGrounded =  hitGround.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.crimson;
        Gizmos.DrawRay(transform.position, Vector2.down * distanceOfGroundJump);
    }
}
