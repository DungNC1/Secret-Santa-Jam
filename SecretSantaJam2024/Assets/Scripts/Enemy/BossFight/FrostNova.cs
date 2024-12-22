using UnityEngine;
using System.Collections;

public class FrostNova : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private float slowDuration = 2f;
    [SerializeField] private float slowAmount = 0.5f;
    [SerializeField] private ParticleSystem frostNovaEffect;
    [SerializeField] private CircleCollider2D frostNovaCollider;
    [SerializeField] private float frostNovaDuration = 3f; // Duration of the FrostNova effect
    private static bool isActive = false; // Static flag to track if FrostNova is active

    private void Start()
    {
        if (isActive)
        {
            Destroy(gameObject);
            return;
        }

        isActive = true;
        frostNovaCollider.radius = 0.1f;

        if (frostNovaEffect != null)
        {
            for(int i = 0; i < 3; i++)
            {
                var effect = Instantiate(frostNovaEffect, transform.position, Quaternion.identity);
                Destroy(effect.gameObject, frostNovaEffect.main.duration);
            }
        }

        StartCoroutine(ExpandCollider());
        StartCoroutine(ApplyEffectsContinuously());
        StartCoroutine(DeactivateAfterDuration());
    }

    private void OnDestroy()
    {
        isActive = false;
    }

    private IEnumerator ExpandCollider()
    {
        float startTime = Time.time;
        float duration = frostNovaDuration;
        float startRadius = frostNovaCollider.radius;
        float endRadius = 5f; // Adjust as needed

        while (Time.time < startTime + duration)
        {
            float t = (Time.time - startTime) / duration;
            frostNovaCollider.radius = Mathf.Lerp(startRadius, endRadius, t);
            yield return null;
        }
    }

    private IEnumerator ApplyEffectsContinuously()
    {
        while (true)
        {
            ApplyEffects();
            yield return new WaitForSeconds(0.5f); // Adjust the frequency as needed
        }
    }

    private void ApplyEffects()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, frostNovaCollider.radius);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                PlayerHealth playerHealth = collider.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damage, transform);
                }

                PlayerMovement playerMovement = collider.GetComponent<PlayerMovement>();
                if (playerMovement != null)
                {
                    playerMovement.StartCoroutine(ApplySlowEffect(playerMovement));
                }
            }
        }
    }

    private IEnumerator ApplySlowEffect(PlayerMovement playerMovement)
    {
        playerMovement.moveSpeed *= slowAmount;
        yield return new WaitForSeconds(slowDuration);
        playerMovement.moveSpeed /= slowAmount;
    }

    private IEnumerator DeactivateAfterDuration()
    {
        yield return new WaitForSeconds(frostNovaDuration);
        isActive = false;
        Destroy(gameObject);
    }
}
