using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SantaCandy : MonoBehaviour
{
    private float moveSpeed;

    private void Start()
    {
        Invoke("DestroySelf", 2.5f);
    }

    private void Update()
    {
        transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(1, this.transform);
            DestroySelf();
        }
    }

    public void UpdateMoveSpeed(float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
