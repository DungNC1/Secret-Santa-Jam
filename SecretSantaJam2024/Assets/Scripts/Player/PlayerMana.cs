using UnityEngine;

public class PlayerMana : MonoBehaviour
{
    public int maxMana = 100;
    private int currentMana;

    void Start()
    {
        currentMana = maxMana;
    }

    public bool UseMana(int amount)
    {
        if (currentMana >= amount)
        {
            currentMana -= amount;
            Debug.Log("Mana used: " + amount + ". Current mana: " + currentMana);
            return true;
        }
        else
        {
            Debug.Log("Not enough mana!");
            return false;
        }
    }

    public void RestoreMana(int amount)
    {
        currentMana = Mathf.Min(currentMana + amount, maxMana);
        Debug.Log("Mana restored: " + amount + ". Current mana: " + currentMana);
    }
}
