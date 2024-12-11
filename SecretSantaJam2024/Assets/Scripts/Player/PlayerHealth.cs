using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 10;
    private int currentHealth;

    private Knockback knockback;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage, Transform damageSource = null)
    {
        currentHealth -= damage;
        Debug.Log(gameObject.name + currentHealth);
        if (currentHealth <= 0)
        {
            knockback.GetKnockedBack(damageSource, 15);
            Debug.Log(gameObject.name + "is dead");
        }
    }
}
