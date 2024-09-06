using UnityEngine;

public class AttractToEnemy : MonoBehaviour
{
    private GameObject targetObject;
    private float attractionSpeed = 30f;
    private float dashDistance = 8f;
    private float maxRange = 8f;

    private float cooldown;
    private bool isDashing = false;
    private bool isHooking = false;
    private Vector2 dashPosition;

    void Update()
    {
        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
        }

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null && hit.collider.CompareTag("Enemy"))
            {
                if (targetObject == null || targetObject != hit.collider.gameObject)
                {
                    targetObject = hit.collider.gameObject;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (cooldown <= 0 && targetObject != null)
            {
                float distanceToEnemy = Vector2.Distance(transform.position, targetObject.transform.position);
                if (distanceToEnemy <= maxRange)
                {
                    StartHooking(targetObject);
                }

                else
                {
                    Debug.Log("Out of Range");
                }
            }

            if (cooldown > 0)
            {
                Cooldowns.cooldowns(cooldown, "Hook");
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (cooldown <= 0 && targetObject != null)
            {
                float distanceToEnemy = Vector2.Distance(transform.position, targetObject.transform.position);
                if (distanceToEnemy <= maxRange)
                {
                    if (distanceToEnemy < dashDistance)
                    {
                        dashDistance = distanceToEnemy;
                    }

                    if (distanceToEnemy <= 4f)
                    {
                        Debug.LogWarning("You are too close.");
                        dashDistance = 10f;
                    }
                    else
                    {
                        StartDashing(targetObject);
                    }
                }
                else
                {
                    Debug.Log("Out of Range");
                }
            }
        }

        if (isHooking)
        {
            targetObject.transform.position = Vector2.MoveTowards(targetObject.transform.position, transform.position, attractionSpeed * Time.deltaTime);
            if (Vector2.Distance(targetObject.transform.position, transform.position) < 2f)
            {
                isHooking = false;
                targetObject = null;
            }
        }

        if (isDashing)
        {
            transform.position = Vector2.MoveTowards(transform.position, dashPosition, attractionSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, dashPosition) < 2f)
            {
                isDashing = false;
                targetObject = null;
                dashDistance = 10f;
            }
        }
    }

    private void StartDashing(GameObject obj)
    {
        cooldown = 2f;
        dashPosition = transform.position + ((obj.transform.position - transform.position).normalized * dashDistance);
        isDashing = true;
    }

    private void StartHooking(GameObject obj)
    {
        cooldown = 2f;
        isHooking = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDashing || isHooking)
        {
            isDashing = false;
            isHooking = false;
            targetObject = null;
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (isDashing || isHooking)
        {
            isDashing = false;
            isHooking = false;
            targetObject = null;
        }
    }
}
