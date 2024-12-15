using UnityEngine;

public class DoorControl : MonoBehaviour
{
    public GameObject[] dummies;
    public GameObject[] doors;

    private void Update()
    {
        CheckDummies();
    }

    private void CheckDummies()
    {
        foreach (GameObject dummy in dummies)
        {
            if (dummy != null)
            {
                return;
            }
        }

        OpenDoors();
    }

    private void OpenDoors()
    {
        foreach (GameObject door in doors)
        {
            Destroy(door); // Remove the doors to open the path
        }
    }
}
