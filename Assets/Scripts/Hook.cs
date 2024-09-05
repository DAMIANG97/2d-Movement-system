using System.Collections;
using UnityEngine;
public class AttractToEnemy : MonoBehaviour
{
    private GameObject targetObject;
    private float attractionSpeed = 30f;
    private float dashDistance = 10f;
    private float maxRange = 12f;

    private float cooldown;
    private bool isDashing = false;
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

        if (Input.GetMouseButtonDown(1))
        {
            if (cooldown <= 0)
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hit.collider != null && hit.collider.gameObject == targetObject)
                {
                    float distanceToEnemy = Vector2.Distance(transform.position, hit.collider.transform.position);
                    if (distanceToEnemy <= maxRange)
                    {
                        if (distanceToEnemy < dashDistance)
                        {
                            dashDistance = distanceToEnemy;
                        }
                        if (distanceToEnemy <= 4f)
                        {
                            Debug.LogWarning("you are to close");
                            dashDistance = 10f;
                        }
                        else
                        {
                            StartDashing(targetObject);
                        }
                    }
                }
            }
            if (cooldown > 0)
            {
                Cooldowns.cooldowns(cooldown, "Dash to enemy");
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
        cooldown = 5f;
        dashPosition = transform.position + ((obj.transform.position - transform.position).normalized * dashDistance);
        isDashing = true;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDashing)
        {
            isDashing = false;
            targetObject = null;
            dashDistance = 10f;
        }
    }
}
