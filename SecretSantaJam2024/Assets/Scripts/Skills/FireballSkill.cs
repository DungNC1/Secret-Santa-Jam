using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Fireball")]
public class FireballSkill : Skill
{
    public GameObject fireballPrefab;
    public Vector3 spawnOffset;
    public float fireballSpeed = 10f;

    public override void UseSkill()
    {
        if (playerMana.UseMana(manaCost))
        {
            Debug.Log("Casting Fireball!");

            // Calculate direction to the mouse cursor
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f; // Ensure the z-coordinate is zero for 2D
            Vector3 direction = (mousePosition - playerMana.transform.position).normalized;

            // Instantiate the fireball at the player's position with an offset
            Vector3 spawnPosition = playerMana.transform.position + direction * spawnOffset.magnitude;
            GameObject fireball = Instantiate(fireballPrefab, spawnPosition, Quaternion.identity);

            // Set the direction and speed of the fireball
            Fireball fireballScript = fireball.GetComponent<Fireball>();
            fireballScript.Initialize(direction, fireballSpeed);
        }
        else
        {
            Debug.Log("Not enough mana to cast Fireball!");
        }
    }
}
