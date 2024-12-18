using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int startingHealth = 3;
    [SerializeField] private GameObject[] loot;
    [HideInInspector] public bool isSleighCharging = false; // New flag

    private int currentHealth;
    private Knockback knockback;
    private PlayerMovement playerMovement;
    private Flash flash;

    private void Awake()
    {
        knockback = GetComponent<Knockback>();
        flash = GetComponent<Flash>();
        playerMovement = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
    }

    private void Start()
    {
        currentHealth = startingHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        
        if(!isSleighCharging)
        {
            knockback.GetKnockedBack(playerMovement.transform, 15f);
        }

        StartCoroutine(flash.FlashRoutine());
    }

    public void DetectDeath()
    {
        if (currentHealth <= 0)
        {
            int chanceOfDroppingLoot = Random.Range(0, 5);

            if(chanceOfDroppingLoot == 1)
            {
                int randomLootIdx = Random.Range(0, loot.Length);
                Instantiate(loot[randomLootIdx], transform.position, Quaternion.identity);
            }

            Destroy(gameObject);
        }
    }
}
