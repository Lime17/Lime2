using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperJump : MonoBehaviour
{
    public KeyCode superJumpKey;  // Changed variable name
    public float jumpForce = 20f;  // Force applied on jump
    public float shockwaveRadius = 5f;  // Radius of the shockwave effect
    public float shockwaveMultiplier = 5f;  // Multiplier for shockwave force

    private Rigidbody2D rb;
    private bool isJumping = false;
    private float jumpStartY;
    private bool wasGroundedLastFrame = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        bool isGrounded = IsGrounded();

        // Start the SuperJump when pressing superJumpKey
        if (Input.GetKeyDown(superJumpKey) && isGrounded)
        {
            Jump();
        }

        // Apply shockwave on landing
        if (isJumping && isGrounded && !wasGroundedLastFrame)
        {
            float fallHeight = jumpStartY - transform.position.y;
            float shockwaveForce = fallHeight * shockwaveMultiplier;
            CreateShockwave(shockwaveForce);
            isJumping = false;
        }

        wasGroundedLastFrame = isGrounded;
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        isJumping = true;
        jumpStartY = transform.position.y;
    }

    void CreateShockwave(float force)
    {
        // Create the shockwave to affect nearby objects
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, shockwaveRadius);
        foreach (Collider2D hit in hits)
        {
            Rigidbody2D hitRb = hit.GetComponent<Rigidbody2D>();
            if (hitRb != null && hitRb != rb)  // Avoid affecting the player itself
            {
                Vector2 direction = (hit.transform.position - transform.position).normalized;
                hitRb.AddForce(direction * force, ForceMode2D.Impulse);
            }
        }
    }

    bool IsGrounded()
    {
        // Simple ground check with raycast
        float extraHeight = 0.1f;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1f + extraHeight, LayerMask.GetMask("Ground"));
        return hit.collider != null;
    }

    void OnDrawGizmosSelected()
    {
        // Visualize shockwave area in editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, shockwaveRadius);
    }
}
