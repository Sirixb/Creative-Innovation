using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health
{
    public event Action OnPlayerDie;

    private readonly int _dieHash = Animator.StringToHash("die");

    protected override void CheckIfDeath()
    {
        base.CheckIfDeath();
        
        if (!IsDeath) return;
        OnPlayerDie?.Invoke();
        animator.SetTrigger(_dieHash);
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (!other.gameObject.TryGetComponent(out EnemyHealth enemy)) return;
        TakeDamage(enemy.DamageByContact, other.transform);
    }
}
