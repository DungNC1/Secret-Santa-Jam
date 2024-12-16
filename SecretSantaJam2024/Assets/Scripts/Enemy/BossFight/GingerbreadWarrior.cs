using UnityEngine;

public class GingerbreadWarrior : MonoBehaviour
{
    [SerializeField] private int candyCaneDamage = 2;
    [SerializeField] private GameObject marshmallowMinionPrefab;
    [SerializeField] private GameObject smallerGingerbreadWarriorPrefab;
    [SerializeField] private float spawnRadius = 5f;
    [SerializeField] private int maxMarshmallowMinions = 3;
    [SerializeField] private int maxSmallerGingerbreadWarriors = 2;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float moveInterval = 3f;
    private float minionSpawnCooldown;
    private float gingerbreadMinioSpawnCooldown;

    private float minionSpawnTimer;
    private Transform player;
    private Vector3 targetPosition;
    private float moveTimer;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        minionSpawnCooldown = Random.Range(5, 10);
        gingerbreadMinioSpawnCooldown = Random.Range(5, 10);
        minionSpawnTimer = minionSpawnCooldown;
        moveTimer = moveInterval;
        SetRandomTargetPosition();
    }

    private void Update()
    {
        minionSpawnTimer -= Time.deltaTime;
        gingerbreadMinioSpawnCooldown -= Time.deltaTime;
        moveTimer -= Time.deltaTime;

        if (minionSpawnTimer <= 0)
        {
            SpawnMarshmallowMinions();
            minionSpawnCooldown = Random.Range(5, 10);
            minionSpawnTimer = minionSpawnCooldown;
        }

        if (gingerbreadMinioSpawnCooldown <= 0)
        {
            SpawnSmallerGingerbreadWarriors();
            gingerbreadMinioSpawnCooldown = Random.Range(5, 10);
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

    private void SpawnMarshmallowMinions()
    {
        for (int i = 0; i < maxMarshmallowMinions; i++)
        {
            Vector3 spawnPosition = GenerateRandomPosition();
            Instantiate(marshmallowMinionPrefab, spawnPosition, Quaternion.identity);
        }
    }

    private void SpawnSmallerGingerbreadWarriors()
    {
        for (int i = 0; i < maxSmallerGingerbreadWarriors; i++)
        {
            Vector3 spawnPosition = GenerateRandomPosition();
            Instantiate(smallerGingerbreadWarriorPrefab, spawnPosition, Quaternion.identity);
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
