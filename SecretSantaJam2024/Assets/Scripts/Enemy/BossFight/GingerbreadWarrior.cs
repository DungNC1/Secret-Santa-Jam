using UnityEngine;

public class GingerbreadWarrior : MonoBehaviour
{
    [SerializeField] private int candyCaneDamage = 2;
    [SerializeField] private GameObject marshmallowMinionPrefab;
    [SerializeField] private GameObject smallerGingerbreadWarriorPrefab;
    [SerializeField] private float spawnRadius = 5f;
    [SerializeField] private int maxMarshmallowMinions = 3;
    [SerializeField] private int maxSmallerGingerbreadWarriors = 2;
    [SerializeField] private Vector2 movementAreaCenter;
    [SerializeField] private Vector2 movementAreaSize;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float moveInterval = 3f;
    [SerializeField] private Vector2 mapBoundsMin;
    [SerializeField] private Vector2 mapBoundsMax;
    private float minionSpawnCooldown;
    private float gingerbreadMinioSpawnCooldown;
    private float slamCooldown;

    private float minionSpawnTimer;
    private Transform player;
    private Vector3 targetPosition;
    private float moveTimer;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        minionSpawnCooldown = Random.Range(5, 15);
        gingerbreadMinioSpawnCooldown = Random.Range(5, 15);
        slamCooldown = Random.Range(10, 15);
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
            minionSpawnCooldown = Random.Range(5, 15);
            minionSpawnTimer = minionSpawnCooldown;
        }

        if(gingerbreadMinioSpawnCooldown <= 0)
        {
            SpawnSmallerGingerbreadWarriors();
            gingerbreadMinioSpawnCooldown = Random.Range(5, 15);
        }

        if (moveTimer <= 0)
        {
            SetRandomTargetPosition();
            moveTimer = moveInterval;
        }

        if(slamCooldown <= 0)
        {
            player.GetComponent<Knockback>().GetKnockedBack(transform, 15);
            slamCooldown = Random.Range(10, 15);
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

        randomPosition.x = Mathf.Clamp(randomPosition.x, mapBoundsMin.x, mapBoundsMax.x);
        randomPosition.y = Mathf.Clamp(randomPosition.y, mapBoundsMin.y, mapBoundsMax.y);

        while (Vector3.Distance(randomPosition, player.position) < 1f)
        {
            randomDirection = Random.insideUnitCircle.normalized * spawnRadius;
            randomPosition = transform.position + randomDirection;
            randomPosition.x = Mathf.Clamp(randomPosition.x, mapBoundsMin.x, mapBoundsMax.x);
            randomPosition.y = Mathf.Clamp(randomPosition.y, mapBoundsMin.y, mapBoundsMax.y);
        }

        return randomPosition;
    }

    private void SetRandomTargetPosition()
    {
        float randomX = Random.Range(-movementAreaSize.x / 2, movementAreaSize.x / 2);
        float randomY = Random.Range(-movementAreaSize.y / 2, movementAreaSize.y / 2);
        targetPosition = movementAreaCenter + new Vector2(randomX, randomY);
    }

    private void MoveTowardsTargetPosition()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(movementAreaCenter, movementAreaSize);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(new Vector2(mapBoundsMin.x, mapBoundsMin.y), new Vector2(mapBoundsMin.x, mapBoundsMax.y));
        Gizmos.DrawLine(new Vector2(mapBoundsMin.x, mapBoundsMin.y), new Vector2(mapBoundsMax.x, mapBoundsMin.y));
        Gizmos.DrawLine(new Vector2(mapBoundsMax.x, mapBoundsMin.y), new Vector2(mapBoundsMax.x, mapBoundsMax.y));
        Gizmos.DrawLine(new Vector2(mapBoundsMin.x, mapBoundsMax.y), new Vector2(mapBoundsMax.x, mapBoundsMax.y));
    }
}
