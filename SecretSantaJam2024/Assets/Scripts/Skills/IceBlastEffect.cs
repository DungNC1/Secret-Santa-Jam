using UnityEngine;

public class IceBlastEffect : MonoBehaviour
{
    public float duration = 2f;

    void Start()
    {
        Destroy(gameObject, duration); // Destroy the ice blast effect after the specified duration
    }
}
