using UnityEngine;

public class Fireball : MonoBehaviour
{
    private Vector3 direction;
    private float speed;

    public void Initialize(Vector3 direction, float speed)
    {
        this.direction = direction;
        this.speed = speed;
        Destroy(gameObject, 5f); // Destroy the fireball after 5 seconds to prevent it from existing indefinitely
    }

    void Update()
    {
        // Move the fireball in the specified direction
        transform.position += direction * speed * Time.deltaTime;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Boss"))
        {
            collision.gameObject.GetComponent<EnemyHealth>().TakeDamage(3);
            Destroy(gameObject);
        }
    }
}
