using UnityEngine;

public class CoalShield : MonoBehaviour
{
    public float orbitRadius = 2f;
    public float orbitSpeed = 5f;
    public float spacingAngle; // This will be set dynamically
    private Transform centerPoint;
    private float currentAngle;

    private void Start()
    {
        Invoke("DestroySelf", 3);
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }

    public void Initialize(Transform center, float initialAngle)
    {
        centerPoint = center;
        currentAngle = initialAngle; // Set the initial angle based on spacing
    }

    private void Update()
    {
        if (centerPoint != null)
        {
            currentAngle += orbitSpeed * Time.deltaTime; // Increment the angle based on orbit speed and time
            float x = centerPoint.position.x + Mathf.Cos(currentAngle) * orbitRadius;
            float y = centerPoint.position.y + Mathf.Sin(currentAngle) * orbitRadius;
            transform.position = new Vector2(x, y);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(1);
            DestroySelf();
        }
    }
}
