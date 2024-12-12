using UnityEngine;

public class EvilElf : MonoBehaviour
{
    [Header("Ability Settings")]
    [SerializeField] private float minAbilityCooldown = 5f;
    [SerializeField] private float maxAbilityCooldown = 10f;

    [Header("Present Barrage")]
    [SerializeField] private GameObject presentPrefab;

    [Header("Toy Bomb")]
    [SerializeField] private GameObject toyBombPrefab;

    [Header("Sleigh Charge")]
    [SerializeField] private Sprite regularSprite;
    [SerializeField] private Sprite sleighSprite;
    private SpriteRenderer spriteRenderer;
    private bool isSleighCharging = false;

    [Header("North Pole Strike")]
    [SerializeField] private GameObject iceSpikePrefab;

    [Header("Elf Minions")]
    [SerializeField] private GameObject hybridMinionPrefab;
    [SerializeField] private int numberOfMinions = 4; // Adjust the number of minions as needed


    [Header("Candy Throw Settings")]
    [SerializeField] private GameObject candyPrefab;
    [SerializeField] private float candyThrowForce = 300f;
    [SerializeField] private int numberOfCandies = 5; // Number of candies to throw
    [SerializeField] private float coneAngle = 45f; // Total angle of the cone in degrees

    [Header("Map Boundaries")]
    [SerializeField] private float mapMinX = -10f;
    [SerializeField] private float mapMaxX = 10f;
    [SerializeField] private float mapMinY = -10f;
    [SerializeField] private float mapMaxY = 10f;

    [Header("Movement Settings")]
    [SerializeField] private Vector2 movementAreaCenter;
    [SerializeField] private Vector2 movementAreaSize;
    [SerializeField] private float moveInterval = 3f;
    [SerializeField] private float moveSpeed = 2f;

    private Transform player;
    private Rigidbody2D rb;
    private float abilityCooldownTimer;
    private float moveTimer;
    private Vector3 targetPosition;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetRandomAbilityCooldown();
        SetRandomTargetPosition();
    }

    private void Update()
    {
        // Ability execution
        abilityCooldownTimer -= Time.deltaTime;
        if (abilityCooldownTimer <= 0)
        {
            ExecuteRandomAbility();
            SetRandomAbilityCooldown();
        }

        // Random roaming
        moveTimer -= Time.deltaTime;
        if (moveTimer <= 0)
        {
            SetRandomTargetPosition();
            moveTimer = moveInterval;
        }
        MoveTowardsTargetPosition();
    }

    private void SetRandomAbilityCooldown()
    {
        abilityCooldownTimer = Random.Range(minAbilityCooldown, maxAbilityCooldown);
    }

    private void ExecuteRandomAbility()
    {
        int randomAbility = Random.Range(0, 6); // We have 6 abilities

        switch (randomAbility)
        {
            case 0:
                ToyBombs();
                break;
            case 1:
                CandyThrow();
                break;
            case 2:
                SummonElfMinions();
                break;
            case 3:
                SleighCharge();
                break;
            case 4:
                PresentBarrage();
                break;
            case 5:
                NorthPoleStrike();
                break;
        }
    }

    // Abilities
    private void ToyBombs()
    {
        Debug.Log("Toy Bombs!");

        for (int i = 0; i < 5; i++)
        {
            Vector3 bombSpawnPosition = transform.position + new Vector3(i * 0.5f, 0, 0); // Adjust offset as needed

            bombSpawnPosition.x = Mathf.Clamp(bombSpawnPosition.x, mapMinX, mapMaxX);
            bombSpawnPosition.y = Mathf.Clamp(bombSpawnPosition.y, mapMinY, mapMaxY);

            GameObject toyBomb = Instantiate(toyBombPrefab, bombSpawnPosition, Quaternion.identity);
            Rigidbody2D rb = toyBomb.GetComponent<Rigidbody2D>();
            Vector2 randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
            rb.AddForce(randomDirection * 5f, ForceMode2D.Impulse); // Ensure force is applied correctly
        }
    }

    private void CandyThrow()
    {
        Debug.Log("Candy Throw!");
        float angleStep = coneAngle / (numberOfCandies - 1);
        float startAngle = -coneAngle / 2;

        for (int i = 0; i < numberOfCandies; i++)
        {
            float angle = startAngle + (angleStep * i);
            Vector3 spawnPosition = transform.position + new Vector3(0, 1, 0); // Adjust position as needed
            GameObject candy = Instantiate(candyPrefab, spawnPosition, Quaternion.identity);
            Rigidbody2D rb = candy.GetComponent<Rigidbody2D>();
            Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
            rb.AddForce(direction * candyThrowForce); // Adjust force as needed
        }
    }


    private void SummonElfMinions()
    {
        Debug.Log("Summon Elf Minions!");
        for (int i = 0; i < numberOfMinions; i++)
        {
            Instantiate(hybridMinionPrefab, transform.position, Quaternion.identity);
        }
    }

    private void SleighCharge()
    {
        Debug.Log("Sleigh Charge!");
        isSleighCharging = true;
        spriteRenderer.sprite = sleighSprite;
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = direction * 10f; // Adjust speed as needed
        GetComponent<EnemyHealth>().isSleighCharging = true;
        Invoke("EndSleighCharge", 2f); // Adjust duration as needed
    }

    private void EndSleighCharge()
    {
        isSleighCharging = false;
        GetComponent<EnemyHealth>().isSleighCharging = false;
        spriteRenderer.sprite = regularSprite;
        rb.velocity = Vector2.zero;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isSleighCharging && collision.collider.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.collider.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(2); // Adjust damage as needed
                Vector2 knockback = (collision.collider.transform.position - transform.position).normalized * 10f;
                collision.collider.GetComponent<Rigidbody2D>().AddForce(knockback, ForceMode2D.Impulse);
            }
            EndSleighCharge();
        }
    }


    private void PresentBarrage()
    {
        Debug.Log("Present Barrage!");
        int numberOfPresents = 7; // Adjust the number of presents as needed
        for (int i = 0; i < numberOfPresents; i++)
        {
            Vector3 spawnPosition = new Vector3(
                Random.Range(movementAreaCenter.x - movementAreaSize.x / 2, movementAreaCenter.x + movementAreaSize.x / 2),
                transform.position.y + 5, // Spawn above the Evil Elf
                0
            );
            GameObject present = Instantiate(presentPrefab, spawnPosition, Quaternion.identity);
            Rigidbody2D rb = present.GetComponent<Rigidbody2D>();
            rb.velocity = new Vector2(0, -5f); // Adjust fall speed as needed
        }
    }


    private void NorthPoleStrike()
    {
        Debug.Log("North Pole Strike!");
        int numberOfSpikes = 3; // Adjust the number of spikes as needed

        for (int i = 0; i < numberOfSpikes; i++)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(mapMinX, mapMaxX), Random.Range(mapMinY, mapMaxY), 0);
            Instantiate(iceSpikePrefab, spawnPosition, Quaternion.identity);
        }
    }


    // Movement
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

        // Draw map boundaries
        Gizmos.color = Color.red;
        Vector3 bottomLeft = new Vector3(mapMinX, mapMinY, 0);
        Vector3 topRight = new Vector3(mapMaxX, mapMaxY, 0);
        Vector3 size = topRight - bottomLeft;
        Gizmos.DrawWireCube(bottomLeft + size / 2, size);
    }
}
