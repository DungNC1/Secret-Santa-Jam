using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    public int damage = 1;  // Damage dealt to the player
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //animator.SetTrigger("Trigger");
            other.GetComponent<PlayerHealth>().TakeDamage(damage, transform);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
