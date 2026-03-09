using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    
    [SerializeField] float currentmoveSpeed = 1f;
    
    [SerializeField] float jumpForce= 10f;
    [SerializeField] float groundCheckDistance = 1f;
    
    [SerializeField] private float coyoteTime = 0.2f;
    [SerializeField] private float jumpBufferTime = 0.2f;
    [SerializeField] private float jumpCutMultiplier = 0.5f;
    [SerializeField] private float acceleration = 1f;
    [SerializeField] private float deceleration = 1f;
    
    [SerializeField] private float wallCheckDistance = 0.5f;
    [SerializeField] private float wallSlideSpeed = 2f;
    [SerializeField] private Vector2 wallJumpForce = new Vector2(8f, 12f);
    [SerializeField] private Vector2 boxSize = new Vector2(0.5f, 0.05f);
    
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheckRight;
    [SerializeField] private Transform wallCheckLeft;

    [SerializeField] private GameObject light;
    
    
    private Vector2 moveInput;
    private Rigidbody2D rb;
    
    private float coyoteTimeCounter;
    private float jumpBufferTimeCounter;
    private float lastJump;
    
    private bool isGrounded;
    private bool isTouchingWall;
    private bool lastTouchingIsWall;
    
    private int wallDirection; // sert a indiquer le coté opposé ou on saute, en gros 1 = droite et -1 c'est a gauche 
    
   
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        
        CheckWall();
        
        
        if (isGrounded)
        {
            if (rb.linearVelocity.y <= -10)
            {
                Debug.Log("mort sale nul");
            }
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        jumpBufferTimeCounter -= Time.deltaTime;
        
        if (jumpBufferTimeCounter > 0f && (coyoteTimeCounter > 0f || isTouchingWall) && Time.time -lastJump > 0.5f)
        {
            Jump();
            lastJump = Time.time;
            jumpBufferTimeCounter = 0f;
        }
    }

    void FixedUpdate()
    {
        Move();
        CheckGround();
    }

    private void Move()
    {
        if (!lastTouchingIsWall)
        {
           // rb.linearVelocity = new Vector2(moveInput.x * currentmoveSpeed, rb.linearVelocity.y); laisse le en com au cas ou si on doit rechanger a l'avenir
           
           float targetSpeed = moveInput.x * currentmoveSpeed;
           float SpeedDiff = targetSpeed - rb.linearVelocity.x;
           float accelerate;

           if (Mathf.Abs(targetSpeed) > 0.01f)
           {
               accelerate = acceleration;
           }
           else
           {
               accelerate = deceleration;
           }
           
           float mouvement = SpeedDiff * accelerate;
           rb.AddForce(Vector2.right * mouvement);
        }
    }

    private void Jump()
    {
        if (isTouchingWall && !isGrounded)
        {
            lastTouchingIsWall = true;
            rb.linearVelocity = new Vector2(-wallDirection * wallJumpForce.x, wallJumpForce.y);
            isTouchingWall = false;
            coyoteTimeCounter = 0f;
            return;
        }
        else
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            coyoteTimeCounter = 0f;
        }
        
        
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            jumpBufferTimeCounter = jumpBufferTime;
        }
        
        // Ajout du Jump cut entre guillemet genre tu sans quand on relache la touche plus tot il saute moin haut
        if (context.canceled && rb.linearVelocity.y > 0f)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * jumpCutMultiplier);
        
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton())
        {
            light.SetActive(true);

        }
        else
        {
            light.SetActive(false);
    
        }
        
    }
    
    private void CheckGround()
    {
        RaycastHit2D hitGround = Physics2D.BoxCast(groundCheck.position, boxSize, 0f, Vector2.down, groundCheckDistance, groundLayer);      
        isGrounded =  hitGround.collider != null;
        if (isGrounded)
        {
            lastTouchingIsWall = false;
        }
    }

    private void CheckWall()
    {
        bool hitRight = Physics2D.Raycast(wallCheckRight.position, Vector2.right, wallCheckDistance, wallLayer);
        bool hitLeft = Physics2D.Raycast(wallCheckLeft.position, Vector2.left, wallCheckDistance, wallLayer);

        isTouchingWall = hitRight || hitLeft;

        if (hitRight)
            wallDirection = 1;
        else if (hitLeft)
            wallDirection = -1;
        
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.crimson;
        Gizmos.DrawWireCube(groundCheck.position, boxSize );
        Gizmos.DrawRay(wallCheckRight.position, Vector2.right * wallCheckDistance);
        Gizmos.DrawRay(wallCheckLeft.position, Vector2.left * wallCheckDistance);
    }
}
