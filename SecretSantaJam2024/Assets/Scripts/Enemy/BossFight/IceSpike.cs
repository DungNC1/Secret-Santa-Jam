using UnityEngine;

public class IceSpike : MonoBehaviour
{
    [SerializeField] private float damage = 25f;
    [SerializeField] private float lifeTime = 2f;

    private void Start()
    {
        Destroy(gameObject, lifeTime); // Destroy the ice spike after a certain time
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage((int)damage);
            }
            Destroy(gameObject); // Destroy the ice spike after hitting the player
        }
    }
}
