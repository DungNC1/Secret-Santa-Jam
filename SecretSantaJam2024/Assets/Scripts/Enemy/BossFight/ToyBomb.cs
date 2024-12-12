using UnityEngine;

public class ToyBomb : MonoBehaviour
{
    [SerializeField] private float explosionDelay = 3f;
    [SerializeField] private float despawnTime = 10f;
    [SerializeField] private float explosionRadius = 2f;
    [SerializeField] private float explosionDamage = 1f;
    private bool isExploding = false;

    private void Start()
    {
        Invoke("Despawn", despawnTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Explode();
        }
    }

    private void Explode()
    {
        if (!isExploding)
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
    }

    private void Despawn()
    {
        if (!isExploding)
        {
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
