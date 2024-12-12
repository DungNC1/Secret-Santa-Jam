using UnityEngine;

public class LightningStrikeEffect : MonoBehaviour
{
    public float duration = 0.5f; // Duration of the effect
    public int damage = 4;

    void Start()
    {
        Destroy(gameObject, duration); // Destroy the effect after the specified duration
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || collision.CompareTag("Boss"))
        {
            EnemyHealth enemy = collision.GetComponent<EnemyHealth>();
            Knockback knockback = collision.GetComponent<Knockback>();
            knockback.enabled = false;

            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }

    public void SetTarget(Vector3 targetPosition)
    {
        transform.position = targetPosition;
    }
}
