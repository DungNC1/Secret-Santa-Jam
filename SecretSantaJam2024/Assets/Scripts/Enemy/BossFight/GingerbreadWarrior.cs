using UnityEngine;

public class GingerbreadWarrior : MonoBehaviour
{
    [SerializeField] private int candyCaneDamage = 2;
    [SerializeField] private GameObject marshmallowMinionPrefab;
    [SerializeField] private GameObject smallerGingerbreadWarriorPrefab;
    [SerializeField] private GameObject bouncingCandyPrefab;
    [SerializeField] private float spawnRadius = 5f;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float moveInterval = 3f;
    private float spawnCooldown;
    private float spawnTimer;

    private Transform player;
    private Vector3 targetPosition;
    private float moveTimer;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        spawnCooldown = Random.Range(1, 5);
        spawnTimer = spawnCooldown;
        moveTimer = moveInterval;
        SetRandomTargetPosition();
    }

    private void Update()
    {
        spawnTimer -= Time.deltaTime;
        moveTimer -= Time.deltaTime;

        if (spawnTimer <= 0)
        {
            SpawnRandomEnemy();
            spawnCooldown = Random.Range(1, 5);
            spawnTimer = spawnCooldown;
        }

        if (moveTimer <= 0)
        {
            SetRandomTargetPosition();
            moveTimer = moveInterval;
        }

        MoveTowardsTargetPosition();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            CandyCaneAttack();
        }
    }

    private void CandyCaneAttack()
    {
        player.GetComponent<PlayerHealth>().TakeDamage(candyCaneDamage, transform);
    }

    private void SpawnRandomEnemy()
    {
        int randomEnemy = Random.Range(0, 3);
        Vector3 spawnPosition = GenerateRandomPosition();

        switch (randomEnemy)
        {
            case 0:
                Instantiate(marshmallowMinionPrefab, spawnPosition, Quaternion.identity);
                break;
            case 1:
                Instantiate(smallerGingerbreadWarriorPrefab, spawnPosition, Quaternion.identity);
                break;
            case 2:
                Instantiate(bouncingCandyPrefab, spawnPosition, Quaternion.identity);
                break;
        }
    }

    private Vector3 GenerateRandomPosition()
    {
        Vector3 randomDirection = Random.insideUnitCircle.normalized * spawnRadius;
        Vector3 randomPosition = transform.position + randomDirection;

        while (Vector3.Distance(randomPosition, player.position) < 1f)
        {
            randomDirection = Random.insideUnitCircle.normalized * spawnRadius;
            randomPosition = transform.position + randomDirection;
        }

        return randomPosition;
    }

    private void SetRandomTargetPosition()
    {
        float randomX = Random.Range(-moveInterval, moveInterval);
        float randomY = Random.Range(-moveInterval, moveInterval);
        targetPosition = new Vector2(transform.position.x + randomX, transform.position.y + randomY);
    }

    private void MoveTowardsTargetPosition()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }
}
