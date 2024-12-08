using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float moveSpeed = 2f;
    public int damage = 1;

    private Transform player;
    private Rigidbody2D rb;
    private Vector2 movement;
    private Knockback knockback;

    // Freeze variables
    private bool isFrozen = false;
    private float freezeTimer = 0f;
    private float originalMoveSpeed;

    private void Awake()
    {
        knockback = GetComponent<Knockback>();
        player = GameObject.FindWithTag("Player").transform;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalMoveSpeed = moveSpeed; // Store the original move speed
    }

    void Update()
    {
        rb.gravityScale = 0; // Ensure gravity is disabled for top-down movement

        // Prioritize knockback logic
        if (knockback.gettingKnockedBack)
        {
            return; // Skip movement and freezing logic if being knocked back
        }

        if (isFrozen)
        {
            freezeTimer -= Time.deltaTime;
            if (freezeTimer <= 0f)
            {
                Unfreeze();
            }
            return;
        }

        if (player != null)
        {
            Vector3 direction = player.position - transform.position;
            direction.Normalize();
            movement = direction;
        }
    }

    void FixedUpdate()
    {
        if (!isFrozen && player != null && !knockback.gettingKnockedBack)
        {
            rb.MovePosition((Vector2)transform.position + (movement * moveSpeed * Time.fixedDeltaTime));
        }
    }

    public void Freeze(float duration)
    {
        isFrozen = true;
        freezeTimer = duration;
        moveSpeed = 0f; // Set move speed to 0 to "freeze" the enemy
        Debug.Log("Enemy frozen!");
        // Additional logic when frozen (e.g., disable animations, change color)
    }

    private void Unfreeze()
    {
        isFrozen = false;
        moveSpeed = originalMoveSpeed; // Restore the original move speed
        Debug.Log("Enemy unfrozen!");
        // Additional logic when unfrozen (e.g., re-enable animations, revert color)
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
    }
}
