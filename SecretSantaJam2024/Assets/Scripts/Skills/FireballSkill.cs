using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Fireball")]
public class FireballSkill : Skill
{
    public GameObject fireballPrefab;
    public float fireballSpeed = 10f;

    public override void UseSkill()
    {
        if (playerMana.UseMana(manaCost))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;
            Vector3 direction = (mousePosition - playerMana.transform.position);

            Debug.Log("Mouse Position: " + mousePosition);
            Debug.Log("Player Position: " + playerMana.transform.position);
            Debug.Log("Direction: " + direction);

            Vector3 spawnPosition = playerMana.transform.position;
            GameObject fireball = Instantiate(fireballPrefab, spawnPosition, Quaternion.identity);
            Fireball fireballScript = fireball.GetComponent<Fireball>();
            fireballScript.Initialize(direction, fireballSpeed);
        }
    }
}
