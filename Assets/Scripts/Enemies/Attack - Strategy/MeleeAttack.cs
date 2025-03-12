using System;
using UnityEngine;

public class MeleeAttack : AttackStrategy
{
    [SerializeField] private Collider2D collider2d;
    public override void Attack(Transform attacker, Transform target)
    {
        collider2d.enabled = true;
    }
    public void DoneAttackingAnimEvent()
    {
        collider2d.enabled = false;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        var playerHealth = other.gameObject.GetComponent<PlayerHealth>();
        playerHealth?.TakeDamage(damage, transform);
    }
}