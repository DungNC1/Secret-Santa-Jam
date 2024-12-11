using UnityEngine;

public class MarshmallowMinion : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float explosionDelay = 3f;
    [SerializeField] private float explosionRadius = 2f;
    [SerializeField] private float explosionDamage = 1f;

    private Transform player;
    private bool isExploding = false;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        Invoke("Explode", explosionDelay);
    }

    private void Update()
    {
        if (player != null && !isExploding)
        {
            // Move towards the player
            Vector2 direction = (player.position - transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        }
    }

    private void Explode()
    {
        isExploding = true;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (Collider2D collider in colliders)
        {
            PlayerHealth playerHealth = collider.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage((int)explosionDamage, transform);
            }
        }
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

}
