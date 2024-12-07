using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSource : MonoBehaviour
{
    [SerializeField] private int damageAmount = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayerHealth>() && other.gameObject.layer == 7)
        {
            PlayerHealth enemyHealth = other.gameObject.GetComponent<PlayerHealth>();
            enemyHealth.TakeDamage(damageAmount);
        }
    }
}
