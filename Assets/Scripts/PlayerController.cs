using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    
    private float currentmoveSpeed = 1f;
    private float jumpForce= 10f;
    private float multiplierStaticJump = 1.3f;
    private float groundCheckDistance = 1f;
    private float coyoteTime = 0.2f;
    private float jumpBufferTime = 0.2f;
    private float jumpCutMultiplier = 0.5f;
    private float acceleration = 1f;
    private float deceleration = 1f;
    private float wallCheckDistance = 0.5f;
    private float wallJumpTired = 2f;
    private float wallJumpTiredMultiplier = 0.5f;
    public float dryCount = 10f;
    public float maxDryCount = 10f;
    private float maxVelocity;
    private float jumpForceLangue;
    
    private Vector2 wallJumpForce = new Vector2(8f, 12f);
    private Vector2 boxSize = new Vector2(0.5f, 0.05f);
    
    private LayerMask groundLayer;
    private LayerMask wallLayer;
    
    [SerializeField] private Transform groundCheck;
    [SerializeField] private  Transform wallCheckRight;
    [SerializeField] private Transform wallCheckLeft;
    
    private Vector2 moveInput;
    private Rigidbody2D rb;
    
    private float coyoteTimeCounter;
    private float jumpBufferTimeCounter;
    private float lastJump;
    private float currentWallJumpY;
    
    private bool isGrounded;
    private bool isTouchingWall;
    private bool lastTouchingIsWall;
    
    private float distance = 2f;
    private bool canLamp = true;
    private float lampTimer = 1f;
    
    [SerializeField] private GameObject pointeur;
    [SerializeField] private GameObject lightCursor;
    [SerializeField] private GameObject lightSphere;
    
    [SerializeField] private LangueControll langueControll;

    private Vector2 joystick = new Vector2();
    
    
    
    private int wallDirection; // sert a indiquer le coté opposé ou on saute, en gros 1 = droite et -1 c'est a gauche 
    
   
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        currentmoveSpeed = playerData.currentmoveSpeed;
        jumpForce = playerData.jumpForce;
        multiplierStaticJump =  playerData.multiplierStaticJump;
        groundCheckDistance = playerData.groundCheckDistance;
        coyoteTime = playerData.coyoteTime;
        jumpBufferTime = playerData.jumpBufferTime;
        jumpCutMultiplier = playerData.jumpCutMultiplier;
        acceleration = playerData.acceleration;
        deceleration = playerData.acceleration;
        wallCheckDistance = playerData.wallCheckDistance;
        wallJumpTired = playerData.wallJumpTired;
        wallJumpTiredMultiplier = playerData.wallJumpTiredMultiplier;
        wallJumpForce = playerData.wallJumpForce; 
        boxSize = playerData.boxSize;
        groundLayer  = playerData.groundLayer;
        wallLayer  = playerData.wallLayer;
        distance = playerData.distance;
        lampTimer = playerData.lampTimer;
        maxDryCount = playerData.dryCount;
        dryCount = maxDryCount;
        maxVelocity = playerData.maxVelocity;

        jumpForceLangue = playerData.jumpForceLangue;
        
        currentWallJumpY = wallJumpForce.y;
        
        
    }

    void Update()
    {
        CheckWall();
        
        jumpBufferTimeCounter -= Time.deltaTime;
        
        if (jumpBufferTimeCounter > 0f && (coyoteTimeCounter > 0f || isTouchingWall) && Time.time -lastJump > 0.5f)
        {
            Jump();
            lastJump = Time.time;
            jumpBufferTimeCounter = 0f;
        }
        
        joystick = Gamepad.current.rightStick.ReadValue();

        if (joystick.magnitude > 0.2f)
        {
            joystick.Normalize();
            
            Vector3 decalage = new Vector3(joystick.x, joystick.y, 0f) * distance ;
            pointeur.transform.position = transform.position + decalage;
            
            float angle = Mathf.Atan2(decalage.y, decalage.x) * Mathf.Rad2Deg; 
            pointeur.transform.rotation = Quaternion.Euler(0f, 0f, angle +90f);
            pointeur.SetActive(true);
        }
        else
        {
            pointeur.SetActive(false);
        }

        dryCount -= 0.01f;
        UIManager.instance.dryCountImage.fillAmount = dryCount / maxDryCount;
        if (dryCount <= 0)
        {
           
            Die();
        }
    }

    void FixedUpdate()
    {
        Move();
        CheckGround();
        if (isGrounded)
        {
            if (rb.linearVelocity.y <= maxVelocity)
            {
                Die();
            }
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
    }

    private void Move()
    {
        if (!lastTouchingIsWall)
        {
           //rb.linearVelocity = new Vector2(moveInput.x * currentmoveSpeed, rb.linearVelocity.y); 
           
           float targetSpeed = moveInput.x * currentmoveSpeed;

           float speedDiff = targetSpeed - rb.linearVelocity.x;
           float accelerate;
           if (isGrounded)
           {
               if (Mathf.Abs(moveInput.x) < 0.01f)
               {
                   rb.linearDamping = 8f;
               }
               else
               {
                   rb.linearDamping = 0f;
               }
           }
           else
           {
               rb.linearDamping = 0f;
           }
           

           if (Mathf.Abs(targetSpeed) > 0.01f)
           {
               accelerate = acceleration;
           }
           else
           {
               accelerate = deceleration;
           }
           
           float mouvement = speedDiff * accelerate;
           
           rb.AddForce(Vector2.right * mouvement, ForceMode2D.Force);
        }
    }

    private void Jump()
    {
        if (isTouchingWall && !isGrounded)
        {
            lastTouchingIsWall = true;
            rb.linearVelocity = new Vector2(-wallDirection * wallJumpForce.x, currentWallJumpY);
            
            currentWallJumpY -= wallJumpTired;
            currentWallJumpY *= wallJumpTiredMultiplier;
            
            if(currentWallJumpY < 0f)
                currentWallJumpY = 0f;
            
            isTouchingWall = false;
            coyoteTimeCounter = 0f;
        }
        else if (!isGrounded && langueControll.wasHoldingTongue )
        {
            langueControll.isGrappling = false;
            langueControll.wasHoldingTongue = false;
            Debug.Log("banane noir");
            Debug.Log(jumpForceLangue);
            rb.AddForce(Vector2.up*jumpForceLangue, ForceMode2D.Impulse);
            coyoteTimeCounter = 0f;
        }
        else if (isGrounded && Mathf.Abs(moveInput.x) < 0.01f && !langueControll.wasHoldingTongue)
        {

            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce * multiplierStaticJump);
            coyoteTimeCounter = 0f;
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
        Vector2 joystick = Gamepad.current.rightStick.ReadValue();
        if (context.ReadValueAsButton() && canLamp)
        {
            if (joystick.magnitude >= 0.1)
            {
                Instantiate(lightCursor, pointeur.transform.position, pointeur.transform.rotation);
            }
            else
            {
                lightSphere.SetActive(true);
            }

            canLamp = false;
            StartCoroutine(LampOffTimer());
        }
    }

    private IEnumerator LampOffTimer()
    {
        yield return new WaitForSeconds(lampTimer);
        canLamp = true;
        lightSphere.SetActive(false);
    }
    
    private void CheckGround()
    {
        RaycastHit2D hitGround = Physics2D.BoxCast(groundCheck.position, boxSize, 0f, Vector2.down, groundCheckDistance, groundLayer);      
        isGrounded =  hitGround.collider != null;
        if (isGrounded)
        {
            lastTouchingIsWall = false;
            currentWallJumpY = wallJumpForce.y;
        }
    }

    private void CheckWall()
    {
        bool hitRight = Physics2D.Raycast(wallCheckRight.position, Vector2.right, wallCheckDistance, groundLayer);
        bool hitLeft = Physics2D.Raycast(wallCheckLeft.position, Vector2.left, wallCheckDistance, groundLayer);

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

    public void Die()
    {
        Destroy(gameObject);
        UIManager.instance.SetDeathMenu(true);
    }
}