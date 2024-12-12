using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float lifeTime;
    [SerializeField] private bool followPlayer = true;
    private float lifeTimeCounter;

    private Transform player;
    private Vector2 target;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        target = new Vector2(player.position.x, player.position.y);

        lifeTimeCounter = lifeTime;
    }

    private void Update()
    {
        if(followPlayer)
        {
            transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
        }

        lifeTimeCounter -= Time.deltaTime;

        if(lifeTimeCounter < 0 )
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(1, transform);
            Destroy(gameObject);
        }
    }
}
