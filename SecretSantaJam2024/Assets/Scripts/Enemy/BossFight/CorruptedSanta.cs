using UnityEngine;
using System.Collections;

public class CorruptedSanta : MonoBehaviour
{
    [SerializeField] private float teleportRangeX = 10f;
    [SerializeField] private float teleportRangeY = 10f;
    [SerializeField] private Vector3 teleportOrigin;
    [SerializeField] private GameObject candyPrefab;
    [SerializeField] private float throwForce = 10f;
    [SerializeField] private int candiesPerBurst = 5;
    private float angleSpread;
    [SerializeField] private int totalBursts = 5;
    [SerializeField] private float startingDistance = 0.1f;
    [SerializeField] private float burstInterval = 0.5f;
    [SerializeField] private Transform player; // Assign the player transform in the inspector
    private float skillCooldown;
    private float skillTimer;
    private bool isShooting = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        SetRandomCooldown();
        skillTimer = skillCooldown;
        teleportOrigin = transform.position;
    }

    private void Update()
    {
        skillTimer -= Time.deltaTime;

        if (skillTimer <= 0)
        {
            UseRandomSkill();
            SetRandomCooldown();
            skillTimer = skillCooldown;
        }
    }

    private void SetRandomCooldown()
    {
        skillCooldown = Random.Range(1, 5);
    }

    private void UseRandomSkill()
    {
        int randomSkill = Random.Range(7, 7);  // Number of skills

        switch (randomSkill)
        {
            case 0:
                PerformCoalBombAttack();
                break;
            case 1:
                PerformCandyCaneStrike();
                break;
            case 2:
                PerformBlizzardSummon();
                break;
            case 3:
                PerformFrostNova();
                break;
            case 4:
                SummonShadowElves();
                break;
            case 5:
                DropDarkGifts();
                break;
            case 6:
                Teleport();
                break;
            case 7:
                PerformThrowingCandies(); // Added Throwing Candies
                break;
        }
    }

    private void PerformCoalBombAttack()
    {
        Debug.Log("Performing Coal Bomb Attack!");
        // Logic for throwing coal bombs
    }

    private void PerformCandyCaneStrike()
    {
        Debug.Log("Performing Candy Cane Strike!");
        // Logic for candy cane melee attack
    }

    private void PerformBlizzardSummon()
    {
        Debug.Log("Summoning Blizzard!");
        // Logic for summoning a blizzard
    }

    private void PerformFrostNova()
    {
        Debug.Log("Performing Frost Nova!");
        // Logic for Frost Nova attack
    }

    private void SummonShadowElves()
    {
        Debug.Log("Summoning Shadow Elves!");
        // Logic for summoning shadow elves
    }

    private void DropDarkGifts()
    {
        Debug.Log("Dropping Dark Gifts!");
        // Logic for dropping dark gifts
    }

    private void Teleport()
    {
        Vector3 randomPosition = new Vector3(
            Random.Range(-teleportRangeX / 2, teleportRangeX / 2),
            Random.Range(-teleportRangeY / 2, teleportRangeY / 2),
            0
        );

        transform.position = teleportOrigin + randomPosition;

        Debug.Log("Teleporting to new position!");
    }

    private void PerformThrowingCandies()
    {
        Debug.Log("Throwing Candies!");
        angleSpread = Random.Range(90, 359);
        StartCoroutine(ShootRoutine());
    }

    private IEnumerator ShootRoutine()
    {
        isShooting = true;

        float startAngle, currentAngle, angleStep;

        TargetConeOfInfluence(out startAngle, out currentAngle, out angleStep);

        for (int i = 0; i < totalBursts; i++)
        {
            for (int j = 0; j < candiesPerBurst; j++)
            {
                Vector2 pos = FindBulletSpawnPos(currentAngle);

                GameObject newBullet = Instantiate(candyPrefab, pos, Quaternion.identity);
                newBullet.transform.right = newBullet.transform.position - transform.position;


                if (newBullet.TryGetComponent(out SantaCandy projectile))
                {
                    projectile.UpdateMoveSpeed(5);
                }

                currentAngle += angleStep;
            }

            currentAngle = startAngle;

            yield return new WaitForSeconds(burstInterval);
            TargetConeOfInfluence(out startAngle, out currentAngle, out angleStep);
        }

        isShooting = false;
    }

    private void TargetConeOfInfluence(out float startAngle, out float currentAngle, out float angleStep)
    {
        Vector2 targetDirection = player.transform.position - transform.position;
        float targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        startAngle = targetAngle;
        float endAngle = targetAngle;
        currentAngle = targetAngle;
        float halfAngleSpread = 0f;
        angleStep = 0;
        if (angleSpread != 0)
        {
            angleStep = angleSpread / (candiesPerBurst - 1);
            halfAngleSpread = angleSpread / 2f;
            startAngle = targetAngle - halfAngleSpread;
            endAngle = targetAngle + halfAngleSpread;
            currentAngle = startAngle;
        }
    }

    private Vector2 FindBulletSpawnPos(float currentAngle)
    {
        float x = transform.position.x + startingDistance * Mathf.Cos(currentAngle * Mathf.Deg2Rad);
        float y = transform.position.y + startingDistance * Mathf.Sin(currentAngle * Mathf.Deg2Rad);

        Vector2 pos = new Vector2(x, y);

        return pos;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(teleportOrigin, new Vector3(teleportRangeX, teleportRangeY, 0));
    }
}