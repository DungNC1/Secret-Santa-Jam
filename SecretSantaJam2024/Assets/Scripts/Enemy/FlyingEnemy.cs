using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    public float speed = 3f;
    public float changeDirectionTime = 3f;
    private Vector3 randomDirection;
    private float timer;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetRandomDirection();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= changeDirectionTime)
        {
            SetRandomDirection();
            timer = 0;
        }

        transform.Translate(randomDirection * speed * Time.deltaTime);
        FaceDirection();
    }

    void SetRandomDirection()
    {
        float randomAngle = Random.Range(0f, 360f);
        randomDirection = new Vector3(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle), 0).normalized;
    }

    void FaceDirection()
    {
        if (randomDirection.x < 0)
        {
            spriteRenderer.flipX = true; // Face left
        }
        else if (randomDirection.x > 0)
        {
            spriteRenderer.flipX = false; // Face right
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(1);
        }
    }
}
