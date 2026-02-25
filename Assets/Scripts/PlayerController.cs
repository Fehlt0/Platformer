using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    
    [SerializeField] float currentSpeed = 1f;
    
    
    [SerializeField] float jumpForce= 10f;
    [SerializeField] float distanceOfGroundJump= 1.05f;
    [SerializeField] float distanceOfWallJump = 1;
    [SerializeField] float jumpDuration = 0.2f;
    [SerializeField] float currentCooldownJump;
    [SerializeField] float cooldownJump = 0f;
    [SerializeField] float initCooldownJump = 0f;
    
    
    private Vector2 moveInput;
    private Transform cameraTransform;
    private Rigidbody2D rb;
    
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
        CheckGround();
        CheckWalls();
        
        currentCooldownJump += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput.x * currentSpeed, rb.linearVelocity.y);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            rb.AddForce(jumpForce *  Vector2.up, ForceMode2D.Impulse);
        }
        else if (isWall && !isGrounded && currentCooldownJump > cooldownJump )
        {
            rb.linearVelocity = new Vector2(-Mathf.Sign(transform.localScale.x)*3,6) ;
            currentCooldownJump = initCooldownJump;
        }
    }


    

    private void CheckGround()
    {
        RaycastHit2D hitGround = Physics2D.Raycast(transform.position, Vector2.down, distanceOfGroundJump, layerMask);
        isGrounded =  hitGround.collider != null;
    }

    private void CheckWalls()
    {
        RaycastHit2D hitWallLeft = Physics2D.Raycast(transform.position, Vector2.left, distanceOfWallJump, layerMask);
        RaycastHit2D hitWallRight = Physics2D.Raycast(transform.position, Vector2.right, distanceOfWallJump, layerMask);
        isWall = hitWallLeft.collider != null;
        if (isWall)
        {
            Debug.Log("Wall");
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector2.down * distanceOfGroundJump);
        Gizmos.DrawRay(transform.position, Vector2.left * distanceOfWallJump);
        Gizmos.DrawRay(transform.position, Vector2.right * distanceOfWallJump);
        
    }
}
