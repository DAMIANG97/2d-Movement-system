using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal;
    private float vertical;
    public float speed = 8f;
    public float dashPower = 20f;
    public float dashTime = 0.2f;
    private float dashCooldown;

    private float dodgeCooldown;
    private bool isDodging;
    private float dodgeTime = 1f;
    private float rotationSpeed = 353f;

    [SerializeField] private Rigidbody2D rb;
    private Collider2D playerCollider;

    private bool isDashing;
    private bool stealth;
    public float stealthSpeedMultiplier = 0.5f;

    void Start()
    {
        playerCollider = GetComponent<Collider2D>();
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (dashCooldown <= 0)
            {
                StartDash();
            }
            else
            {
                Cooldowns.cooldowns(dashCooldown, "Dash");
            }
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (dodgeCooldown <= 0)
            {
                StartDodge();
            }
            else
            {
                Cooldowns.cooldowns(dodgeCooldown, "Dodge");
            }
        }


        if (Input.GetKey(KeyCode.C))
        {
            stealth = true;
        }
        else
        {
            stealth = false;
        }

        if (dashCooldown > 0)
        {
            dashCooldown -= Time.deltaTime;
        }
        if (dodgeCooldown > 0)
        {
            dodgeCooldown -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        Vector2 movement = new Vector2(horizontal, vertical).normalized;


        if (isDashing)
        {
            rb.velocity = movement * dashPower;
            dashTime -= Time.fixedDeltaTime;
            if (dashTime <= 0)
            {
                isDashing = false;
                rb.velocity = Vector2.zero;
            }
        }

        else if (isDodging)
        {
            float rotationAmount = rotationSpeed * Time.fixedDeltaTime;
            transform.Rotate(rotationAmount, 0, 0);

            dodgeTime -= Time.fixedDeltaTime;
            if (dodgeTime <= 0)
            {
                isDodging = false;
                playerCollider.enabled = true;
            }
        }

        else
        {
            float currentSpeed = stealth ? speed * stealthSpeedMultiplier : speed;
            rb.velocity = movement * currentSpeed;

            if (movement != Vector2.zero)
            {
                float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
            }
        }
    }

    private void StartDash()
    {
        isDashing = true;
        dashTime = 0.2f;
        dashCooldown = 2f;
    }

    private void StartDodge()
    {
        isDodging = true;
        dodgeTime = 1f;
        dodgeCooldown = 10.0f;
        rb.velocity = Vector2.zero;
        playerCollider.enabled = false;
    }
}
