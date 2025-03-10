using System;
using UnityEngine;

public class MeleeAttack : AttackStrategy
{
    private Collider2D _collider2D;
    public override void Attack(Transform attacker, Transform target, Collider2D collider2d)
    {
        _collider2D = collider2d;
        _collider2D.enabled = true;
    }
    public void DoneAttackingAnimEvent()
    {
        _collider2D.enabled = false;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        var playerHealth = other.gameObject.GetComponent<PlayerHealth>();
        playerHealth?.TakeDamage(damage, transform);
    }
}