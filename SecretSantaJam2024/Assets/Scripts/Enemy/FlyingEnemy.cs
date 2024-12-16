using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    public float speed = 3f;

    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        Patrol();
    }

    void Patrol()
    {
        // Simple back and forth patrol behavior
        transform.position = initialPosition + new Vector3(Mathf.PingPong(Time.time * speed, 4) - 2, Mathf.Sin(Time.time * speed) * 2, 0);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(1);
        }
    }
}
