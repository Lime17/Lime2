using UnityEngine;
using System.Collections;

[AddComponentMenu("Playground/Movement/Dash")]
[RequireComponent(typeof(Rigidbody2D))]
public class Dash : Physics2DObject
{
    private Vector2 dashDirection;
    private Vector2 movement, cachedDirection;
    private float moveHorizontal;
    private float moveVertical;
    private float lastDashTime = -Mathf.Infinity;

    [Header("Input keys")]
    public Enums.KeyGroups typeOfControl = Enums.KeyGroups.ArrowKeys;

    [Header("Movement")]
    public float speed = 7f;
    public Enums.MovementType movementType = Enums.MovementType.AllDirections;

    [Header("Orientation")]
    public bool orientToDirection = false;
    public Enums.Directions lookAxis = Enums.Directions.Up;

    [Header("Dash Settings")]
    public float dashForce = 20f;
    public float dashDuration = 0.1f;
    public float dashCooldown = 2f;
    public KeyCode dashKey = KeyCode.LeftShift;

    private bool isDashing = false;

    void Update()
    {
        // Input handling
        if (typeOfControl == Enums.KeyGroups.ArrowKeys)
        {
            moveHorizontal = Input.GetAxis("Horizontal");
            moveVertical = Input.GetAxis("Vertical");
        }
        else
        {
            moveHorizontal = Input.GetAxis("Horizontal2");
            moveVertical = Input.GetAxis("Vertical2");
        }

        // Movement type restrictions
        switch (movementType)
        {
            case Enums.MovementType.OnlyHorizontal:
                moveVertical = 0f;
                break;
            case Enums.MovementType.OnlyVertical:
                moveHorizontal = 0f;
                break;
        }

        movement = new Vector2(moveHorizontal, moveVertical);

        // Debug key input
        if (Input.GetKeyDown(dashKey))
        {
            Debug.Log($"🟢 Dash key '{dashKey}' pressed | Time: {Time.time} | Movement: {movement}");
        }

        // Attempt dash
        if (Input.GetKeyDown(dashKey) && Time.time >= lastDashTime + dashCooldown && movement != Vector2.zero)
        {
            Debug.Log("⚡ Dash triggered!");
            StartCoroutine(PerformDash());
        }

        // Orientation logic
        if (orientToDirection && movement.sqrMagnitude >= 0.01f)
        {
            cachedDirection = movement;
            Utils.SetAxisTowards(lookAxis, transform, cachedDirection);
        }
    }

    void FixedUpdate()
    {
        if (isDashing)
        {
            rigidbody2D.velocity = dashDirection * dashForce;
        }
        else
        {
            rigidbody2D.AddForce(movement * speed * 10f);
        }
    }

    private IEnumerator PerformDash()
    {
        isDashing = true;
        lastDashTime = Time.time;
        dashDirection = movement.normalized;
        rigidbody2D.velocity = dashDirection * dashForce;

        yield return new WaitForSeconds(dashDuration);

        isDashing = false;
    }

    // Setter method to be used by FightManager or any other script
    public void SetDashKey(KeyCode key)
    {
        dashKey = key;
        Debug.Log($"📥 Dash key set to {dashKey} for {gameObject.name}");
    }
}
