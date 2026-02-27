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
    [SerializeField] private float airTimeKill = 1f;
    
    [SerializeField] private float wallCheckDistance = 0.5f;
    [SerializeField] private float wallSlideSpeed = 2f;
    [SerializeField] private Vector2 wallJumpForce = new Vector2(8f, 12f);
    
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheckRight;
    [SerializeField] private Transform wallCheckLeft;
    
    private Vector2 moveInput;
    private Rigidbody2D rb;
    
    private float coyoteTimeCounter;
    private float jumpBufferTimeCounter;
    private float lastJump;
    private float airTime;
    
    private bool isGrounded;
    private bool isTouchingWall;
    private bool isWallSliding;
    
    private int wallDirection; // sert a indiquer le coté opposé ou on saute, en gros 1 = droite et -1 c'est a gauche 
    
   
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        CheckGround();
        CheckWall();
        WallSlide();
        
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
        
        if (jumpBufferTimeCounter > 0f && (coyoteTimeCounter > 0f || isWallSliding) && Time.time -lastJump > 0.5f)
        {
            Jump();
            lastJump = Time.time;
            jumpBufferTimeCounter = 0f;
        }
    }

    void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        rb.linearVelocity = new Vector2(moveInput.x * currentmoveSpeed, rb.linearVelocity.y);
    }

    private void Jump()
    {
        
        if (isWallSliding)
        {
            rb.linearVelocity = new Vector2(-wallDirection * wallJumpForce.x, wallJumpForce.y);
            isWallSliding =  false;
            coyoteTimeCounter = 0f;
            return;
            
        }
        
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        coyoteTimeCounter = 0f;
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
    

    

    private void CheckGround()
    {
        RaycastHit2D hitGround = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);
        isGrounded =  hitGround.collider != null;
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

    private void WallSlide()
    {
        if (isTouchingWall && !isGrounded)
        {
            isWallSliding = true;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Clamp(rb.linearVelocity.y, -wallSlideSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }

    
    
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.crimson;
        Gizmos.DrawRay(groundCheck.position, Vector2.down * groundCheckDistance );
        Gizmos.DrawRay(wallCheckRight.position, Vector2.right * wallCheckDistance);
        Gizmos.DrawRay(wallCheckLeft.position, Vector2.left * wallCheckDistance);
    }
}
