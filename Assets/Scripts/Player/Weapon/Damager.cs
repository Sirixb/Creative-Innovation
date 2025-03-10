using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damager : MonoBehaviour
{
    private int _damage;

    public void Config(int damage)
    {
       _damage = damage;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<EnemyHealth>(out var enemyHealth))
        {
            // Debug.Log("Detecto enemigo"+ other.name);
            enemyHealth.TakeDamage(_damage, transform);
        }
    }
}
