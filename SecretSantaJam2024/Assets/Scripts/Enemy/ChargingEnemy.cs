using System.Collections;
using UnityEngine;

public class ChargingEnemy : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float chargeSpeed = 10f;
    public float chargeDistance = 5f;
    public float chargeCooldown = 2f;

    private Transform player;
    private Rigidbody2D rb;
    private Vector2 movement;
    private bool isCharging = false;
    private float chargeTimer = 0f;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isCharging)
        {
            return; // Skip movement logic if charging
        }

        if (player != null)
        {
            Vector3 direction = player.position - transform.position;
            direction.Normalize();
            movement = direction;

            // Check if within charge distance
            if (Vector3.Distance(transform.position, player.position) <= chargeDistance && chargeTimer <= 0f)
            {
                StartCharge();
            }
        }

        // Update charge timer
        if (chargeTimer > 0f)
        {
            chargeTimer -= Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        if (isCharging)
        {
            rb.MovePosition(rb.position + movement * chargeSpeed * Time.fixedDeltaTime);
        }
        else if (player != null)
        {
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
    }

    private void StartCharge()
    {
        isCharging = true;
        chargeTimer = chargeCooldown; // Reset charge cooldown
        StartCoroutine(ChargeRoutine());
    }

    private IEnumerator ChargeRoutine()
    {
        yield return new WaitForSeconds(0.5f); // Adjust charge duration as needed
        isCharging = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(1, transform);
        }
    }
}
