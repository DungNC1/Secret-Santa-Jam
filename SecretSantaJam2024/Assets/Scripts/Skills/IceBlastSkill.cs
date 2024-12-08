using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Ice Blast")]
public class IceBlastSkill : Skill
{
    public GameObject iceBlastEffectPrefab;
    public float freezeDuration = 3f;
    public LayerMask enemyLayers;

    public override void UseSkill()
    {
        if (playerMana.UseMana(manaCost))
        {
            Debug.Log("Casting Ice Blast!");

            // Step 1: Calculate the direction the player is facing
            Vector3 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - playerMana.transform.position;
            direction.z = 0; // Ensure the z-coordinate is zero for 2D
            direction.Normalize(); // Normalize the direction vector

            // Step 2: Instantiate the ice blast effect in front of the player
            Vector3 spawnPosition = playerMana.transform.position + direction * 0.5f;
            GameObject iceBlastEffect = Instantiate(iceBlastEffectPrefab, spawnPosition, Quaternion.LookRotation(Vector3.forward, direction));

            // Step 3: Draw the raycast for debugging purposes
            Debug.DrawRay(spawnPosition, direction * 10f, Color.blue, 2f); // Draws a blue ray in the scene view

            // Step 4: Perform the raycast to detect the first enemy hit
            RaycastHit2D hit = Physics2D.Raycast(spawnPosition, direction, Mathf.Infinity, enemyLayers);
            if (hit.collider != null && hit.collider.CompareTag("Enemy"))
            {
                // Step 5: Apply the freezing effect to the hit enemy
                EnemyMovement enemy = hit.collider.GetComponent<EnemyMovement>();
                if (enemy != null)
                {
                    enemy.Freeze(freezeDuration);
                }
            }
        }
        else
        {
            Debug.Log("Not enough mana to cast Ice Blast!");
        }
    }
}
