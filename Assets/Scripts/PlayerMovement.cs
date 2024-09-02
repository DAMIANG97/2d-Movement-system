using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal;
    private float vertical;
    public float speed = 4f;
    public float dashPower = 6f;
    public float dashTime = 0.2f;
    private float dashCooldown;

    private float dodgeCooldown;
    private bool isDodging;
    private float dodgeTime = 1f;
    private float rotationSpeed = 353f;

    [SerializeField] private Rigidbody2D rb;
    private Collider2D playerCollider;

    private bool isDashing;

    void Start()
    {
        playerCollider = GetComponent<Collider2D>();
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal") * speed;
        vertical = Input.GetAxisRaw("Vertical") * speed;

        if (Input.GetKeyDown(KeyCode.Space) && dashCooldown <= 0)
        {
            StartDash();
        }

        if (Input.GetKeyDown(KeyCode.X) && dodgeCooldown <= 0)
        {
            StartDodge();
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
        if (isDashing)
        {
            rb.velocity = new Vector3(horizontal * dashPower, vertical * dashPower);
            dashTime -= Time.fixedDeltaTime;
            if (dashTime <= 0)
            {
                isDashing = false;
                rb.velocity = new Vector3(horizontal, vertical);
            }
        }
        else if (isDodging)
        {
            float rotationAmount = rotationSpeed * Time.fixedDeltaTime;
            transform.Rotate(rotationAmount, 0, 0);

            dodgeTime -= Time.deltaTime;
            if (dodgeTime <= 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
                isDodging = false;
                playerCollider.enabled = true;
            }
        }
        else
        {
            rb.velocity = new Vector3(horizontal, vertical);
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
