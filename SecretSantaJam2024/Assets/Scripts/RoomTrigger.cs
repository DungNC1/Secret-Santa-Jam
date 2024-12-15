using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    public GameObject[] enemies; // Array to hold the room's enemies

    void Start()
    {
        // Disable all enemies at the start
        SetEnemiesActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Enable enemies when the player enters the room
            SetEnemiesActive(true);
        }
    }

    void SetEnemiesActive(bool isActive)
    {
        foreach (GameObject enemy in enemies)
        {
            enemy.SetActive(isActive);
        }
    }
}
