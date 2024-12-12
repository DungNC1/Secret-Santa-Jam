using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField] private Image healthFill;
    [SerializeField] private Image manaFill;
    private PlayerHealth playerHealth;
    private PlayerMana playerMana;

    private void Start()
    {
        playerHealth = GameObject.FindWithTag("Player").GetComponent<PlayerHealth>();
        playerMana = GameObject.FindWithTag("Player").GetComponent<PlayerMana>();

        UpdateHealth();
        UpdateMana();
    }

    private void Update()
    {
        UpdateHealth();
        UpdateMana();
    }

    private void UpdateHealth()
    {
        healthFill.fillAmount = (float)playerHealth.currentHealth / playerHealth.maxHealth;
    }

    private void UpdateMana()
    {
        manaFill.fillAmount = (float)playerMana.currentMana / playerMana.maxMana;
    }
}
