using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
public class PlayerData : ScriptableObject
{
    public float currentmoveSpeed = 6f;
    
    public float airControlSpeed = 1f;
    
    public float jumpForce = 10f;
    public float multiplierStaticJump = 1.3f;
    public float groundCheckDistance = 0.18f;
    public float coyoteTime = 0.2f;
    public float jumpBufferTime = 0.2f;
    public float jumpCutMultiplier = 0.5f;
    public float acceleration = 1f;
    public float deceleration = 1f;
    
    public float wallSlideSpeed = 2f;
    public float wallJumpControlLockTime = 0.15f;
    public float wallCheckDistance = 0.5f;
    public float wallJumpTired = 2f;
    public float wallJumpTiredMultiplier = 0.5f;
    public float dryCount = 10f;
    public float maxVelocity = -10f;
    
    public float distance = 2f;
    public float lampTimer = 1f;

    public float jumpForceLangue = 5f;
    
    public Vector2 wallJumpForce = new Vector2(8f, 12f);
    public Vector2 boxSize = new Vector2(0.5f, 0.05f);
    
    public LayerMask groundLayer;
    public LayerMask wallLayer;
    
}
