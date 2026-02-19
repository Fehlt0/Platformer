using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    
    [SerializeField] float currentSpeed = 1f;
    [SerializeField] float jumpForce= 10f;
    [SerializeField] float distanceOfGroundJump= 1.05f;
    
    private Vector2 moveInput;
    private Transform cameraTransform;
    private Rigidbody2D rb;
    
    private bool isGrounded;
    
    public LayerMask layerMask;
    
    
   
    void Start()
    {
        cameraTransform = Camera.main.transform;
        rb = GetComponent<Rigidbody2D>();

    }

    void Update()
    {
        CheckGround();
    }

    private void FixedUpdate()
    {
        Vector2 forward =  cameraTransform.forward;
        Vector2 right =  cameraTransform.right;
        
        forward.y = 0;
        right.y = 0;
        
        forward.Normalize();
        right.Normalize();
        
        
        Vector2 direction = forward * moveInput.y + right * moveInput.x;

        if (direction.magnitude > 0.0001f)
        {
            transform.Translate( direction * (currentSpeed * Time.deltaTime), Space.World);
            
        }
        /*rb.linearVelocity = new Vector2(moveInput.x * currentSpeed, rb.linearVelocity.y);
         
        autre solution possible sans la caméra mais il y a un bug a resoudre, en gros quand on saute vers le coté d'un 
        element considere comme un sol bas on reste bloque dans les air #flemme de le resoudre cordialement */
        
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
        
    }

    private void CheckGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, distanceOfGroundJump, layerMask);
        isGrounded =  hit.collider != null;
    }
}
