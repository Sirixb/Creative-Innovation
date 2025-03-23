using System;
using System.Collections;
using UnityEngine;

public class MeleeAttack : AttackStrategy
{
    [SerializeField] private Collider2D collider2d;
    [SerializeField] private float timeToFinishMeleeAttack = .5f;

    public override void Attack(Transform attacker, Transform target)
    {
        collider2d.enabled = true;
        StartCoroutine(FinishMeleeAttack());
    }

    private IEnumerator FinishMeleeAttack()
    {
        yield return new WaitForSeconds(timeToFinishMeleeAttack);
        collider2d.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var playerHealth = other.gameObject.GetComponent<PlayerHealth>();
        playerHealth?.TakeDamage(damage, transform);
        // if (playerHealth)
        //     Debug.Log($"melee damage {damage} + attackRange {attackRange}+ attackRate {attackRate}");
    }
}