using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBoost : MonoBehaviour
{
    [SerializeField] private Boost boostType;

    private enum Boost
    {
        Health,
        Mana,
        Speed
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            switch(boostType)
            {
                case Boost.Health:
                    collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(-1, transform);
                    break;
                case Boost.Mana:
                    collision.gameObject.GetComponent<PlayerMana>().RestoreMana(20);
                    break;
                case Boost.Speed:
                    StartCoroutine(collision.gameObject.GetComponent<PlayerMovement>().MovementBoost(8));
                    break;
            }

            Destroy(gameObject);
        }
    }
}
