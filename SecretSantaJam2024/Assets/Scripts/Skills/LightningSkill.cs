using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Skills/Lightning Strike")]
public class LightningStrikeSkill : Skill
{
    public GameObject lightningStrikeEffectPrefab;
    public float damage = 50f;
    public float strikeDelay = 0.1f;
    public int maxEnemies = 5;

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
                // Instantiate the lightning strike effect at the enemy's position after a short delay
                CoroutineHelper.Instance.StartCoroutine(SpawnLightningStrike(enemies[i].transform.position, strikeDelay * i));
            }
        }
        else
        {
            Debug.Log("Not enough mana to cast Lightning Strike!");
        }
    }

    private IEnumerator SpawnLightningStrike(Vector3 targetPosition, float delay)
    {
        yield return new WaitForSeconds(delay);

        // Instantiate the lightning strike effect
        GameObject lightningStrikeEffect = Instantiate(lightningStrikeEffectPrefab, targetPosition, Quaternion.identity);

        // Optionally, set the target position for visual alignment if needed
        LightningStrikeEffect effectScript = lightningStrikeEffect.GetComponent<LightningStrikeEffect>();
        if (effectScript != null)
        {
            effectScript.SetTarget(targetPosition);
        }
    }
}
