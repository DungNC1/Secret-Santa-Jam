using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private float moveSpeed;

    [Header("Slash Effect")]
    [SerializeField] private GameObject slashAnimPrefab;
    [SerializeField] private Transform slashAnimSpawnPoint;

    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private int damage = 1;
    [SerializeField] private Animator swordAnimator;

    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer;
    private float cooldownTimer = Mathf.Infinity;

    private Transform player;
    private Rigidbody2D rb;
    private Vector2 movement;
    private Knockback knockback;
    private PlayerHealth playerHealth;
    private GameObject slashAnim;

    private bool isFacingLeft;

    // Freeze variables
    private bool isFrozen = false;
    private float freezeTimer = 0f;
    private float originalMoveSpeed;
    private bool isAttacking = false;

    private void Awake()
    {
        moveSpeed = Random.Range(2f, 4f);
        Debug.Log(moveSpeed);
        knockback = GetComponent<Knockback>();
        player = GameObject.FindWithTag("Player").transform;
        InvokeRepeating("ResetAttack", 1, 1);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalMoveSpeed = moveSpeed; // Store the original move speed
    }

    void Update()
    {
        rb.gravityScale = 0;
        cooldownTimer += Time.deltaTime;

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

        if (movement.x > 0.01f)
        {
            Vector3 localScale = transform.localScale;
            localScale.x = Mathf.Abs(localScale.x);
            localScale.x = Mathf.Abs(localScale.x);
            transform.localScale = localScale;
        }
        else if (movement.x < -0.01f)
        {
            Vector3 localScale = transform.localScale;
            localScale.x = -Mathf.Abs(localScale.x);
            transform.localScale = localScale;
        }


        isFacingLeft = movement.x < 0;

        if (PlayerInSight())
        {
            isAttacking = true;

            if (cooldownTimer >= attackCooldown)
            {
                cooldownTimer = 0;
                swordAnimator.SetTrigger("Attack");
                playerHealth.TakeDamage(damage, transform);
                slashAnim = Instantiate(slashAnimPrefab, slashAnimSpawnPoint.position, Quaternion.identity);
                slashAnim.GetComponent<SpriteRenderer>().flipX = isFacingLeft;
                slashAnim.transform.parent = this.transform.parent;
                isAttacking = false;
            }
        }
    }

    void FixedUpdate()
    {
        if (!isFrozen && player != null && !knockback.gettingKnockedBack && !isAttacking)
        {
            rb.MovePosition((Vector2)transform.position + (movement * moveSpeed * Time.fixedDeltaTime));
        }
    }

    private void ResetAttack()
    {
        isAttacking = false;
    }

    public void Freeze(float duration)
    {
        isFrozen = true;
        freezeTimer = duration;
        moveSpeed = 0f; // Set move speed to 0 to "freeze" the enemy
        Debug.Log("Enemy frozen!");
    }

    private void Unfreeze()
    {
        isFrozen = false;
        moveSpeed = originalMoveSpeed; // Restore the original move speed
        Debug.Log("Enemy unfrozen!");
    }

    private bool PlayerInSight()
    {
        RaycastHit2D hit =
            Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        if (hit.collider != null)
            playerHealth = hit.transform.GetComponent<PlayerHealth>();

        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }
}
