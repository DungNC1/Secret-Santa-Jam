using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    public float speed = 3f;
    public float changeDirectionTime = 3f;
    private Vector3 randomDirection;
    private float timer;

    void Start()
    {
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
    }

    void SetRandomDirection()
    {
        float randomAngle = Random.Range(0f, 360f);
        randomDirection = new Vector3(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle), 0).normalized;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(1);
        }
    }
}
