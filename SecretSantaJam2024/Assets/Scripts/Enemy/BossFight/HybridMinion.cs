using UnityEngine;

public class HybridMinion : MonoBehaviour
{
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float shieldRadius = 2f;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float spreadDistance = 2f; // Distance to spread around the boss
    private Transform boss;
    private Transform player;
    private Vector3 targetPosition;

    private void Awake()
    {
        boss = GameObject.FindWithTag("Boss").transform;
        player = GameObject.FindWithTag("Player").transform;
    }

    private void Start()
    {
        // Calculate a random spread position around the boss
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        targetPosition = boss.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * spreadDistance;
    }

    private void Update()
    {
        MoveTowardsTargetPosition();
        CheckForPlayer();
    }

    private void MoveTowardsTargetPosition()
    {
        // Move towards the calculated target position
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    private void CheckForPlayer()
    {
        if (Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            AttackPlayer();
        }
    }

    private void AttackPlayer()
    {
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage((int)attackDamage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, shieldRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
