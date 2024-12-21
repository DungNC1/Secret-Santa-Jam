using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 10;
    public int currentHealth { get; private set; }

    private Knockback knockback;
    private Animator animator;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        knockback = GetComponent<Knockback>();
    }

    public void TakeDamage(int damage, Transform damageSource = null)
    {
        currentHealth -= damage;
        knockback.GetKnockedBack(damageSource, 7);
        Debug.Log(gameObject.name + currentHealth);

        if (currentHealth <= 0)
        {
            animator.SetTrigger("Die");   
            Debug.Log(gameObject.name + "is dead");
        }
    }

    public void ReloadGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
