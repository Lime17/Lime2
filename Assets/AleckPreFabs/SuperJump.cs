using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SuperJump : MonoBehaviour
{
    public KeyCode superJumpKey = KeyCode.None;      // Assigned via FightManager
    public float jumpForce = 20f;                    // Upward impulse
    public float shockwaveRadius = 5f;               // Radius of the landing shockwave
    public float shockwaveMultiplier = 5f;           // Force per unit fall height
    public float groundCheckDistance = 1.1f;         // How far down to raycast

    public float superJumpCooldown = 2f;  // Cooldown time
    private bool canSuperJump = true;      // Flag to check if jump can be used


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

        // Try to SuperJump
       if (Input.GetKeyDown(superJumpKey) && isGrounded && canSuperJump)
        {
            Jump();
            StartCoroutine(SuperJumpCooldown());
        }


        // On landing, trigger shockwave
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
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, shockwaveRadius);
        foreach (Collider2D hit in hits)
        {
            Rigidbody2D hitRb = hit.attachedRigidbody;
            if (hitRb != null && hitRb != rb)
            {
                Vector2 dir = (hit.transform.position - transform.position).normalized;
                hitRb.AddForce(dir * force, ForceMode2D.Impulse);
            }
        }
    }

    bool IsGrounded()
    {
        Vector2 origin = new Vector2(transform.position.x, transform.position.y - 0.5f); // Adjust origin to feet
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, groundCheckDistance);

        if (hit.collider != null)
        {
            // Debugging
            Debug.Log("Raycast hit: " + hit.collider.name + " (tag: " + hit.collider.tag + ")");
            return hit.collider.CompareTag("Ground");
        }

        return false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, shockwaveRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCheckDistance);
    }

    private IEnumerator SuperJumpCooldown()
    {
        canSuperJump = false; // Disable super jump temporarily
        yield return new WaitForSeconds(superJumpCooldown); // Wait for cooldown to expire
        canSuperJump = true;  // Enable super jump again
    }

}

