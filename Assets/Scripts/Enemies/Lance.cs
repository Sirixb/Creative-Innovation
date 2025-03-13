using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lance : MonoBehaviour
{
    public int Damage { get; set; }

    private void Start()
    {
        Destroy(gameObject,2f);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        var playerHealth = other.gameObject.GetComponent<PlayerHealth>();
        playerHealth?.TakeDamage(Damage, transform);
        Destroy(gameObject);
    }
}
