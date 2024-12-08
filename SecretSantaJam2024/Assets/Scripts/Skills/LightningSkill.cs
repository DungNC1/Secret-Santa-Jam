using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Lightning Strike")]
public class LightningStrikeSkill : Skill
{
    public GameObject lightningStrikeEffectPrefab;
    public int damage = 3;
    public float strikeDelay = 0.1f;
    public int maxEnemies = 5;
    public LayerMask enemyLayers;

    public override void UseSkill()
    {
        if (playerMana.UseMana(manaCost))
        {
            Debug.Log("Casting Lightning Strike!");

            // Find all enemies in the scene
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            // Shuffle the array to ensure randomness
            for (int i = enemies.Length - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                GameObject temp = enemies[i];
                enemies[i] = enemies[j];
                enemies[j] = temp;
            }

            // Strike up to maxEnemies
            int enemiesToHit = Mathf.Min(maxEnemies, enemies.Length);
            for (int i = 0; i < enemiesToHit; i++)
            {
                // Apply the lightning strike effect after a short delay
                Vector3 strikePosition = enemies[i].transform.position + Vector3.up * 10f; // Adjust the height as needed
                Instantiate(lightningStrikeEffectPrefab, strikePosition, Quaternion.identity);
                
            }
        }
        else
        {
            Debug.Log("Not enough mana to cast Lightning Strike!");
        }
    }

    private IEnumerator ApplyLightningDamage(Vector3 targetPosition, float delay)
    {
        yield return new WaitForSeconds(delay);

        // Check if the enemy is still there
        Collider2D hit = Physics2D.OverlapPoint(targetPosition, enemyLayers);
        if (hit != null && hit.CompareTag("Enemy"))
        {
            PlayerHealth enemy = hit.GetComponent<PlayerHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
    }
}
