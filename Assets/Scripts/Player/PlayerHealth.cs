using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private Animator animator;
    [SerializeField] private int currentHealth = 100;
    [SerializeField] private bool canTakeDamage = true;
    [SerializeField] private float invulnerabilityRecoveryTime = 1f;
    private bool _isDeath = false;
    public event Action OnPlayerDie;

    private readonly int _dieHash = Animator.StringToHash("die");

    public void TakeDamage(int damageAmount, Transform hitTransform)
    {
        if (_isDeath || !canTakeDamage)
        {
            return;
        }

        canTakeDamage = false;
        currentHealth -= damageAmount;
        StartCoroutine(DamageRecoveryRoutine());
        CheckIfPlayerDeath();
    }

    private void CheckIfPlayerDeath()
    {
        if (currentHealth > 0 || _isDeath) return;
        _isDeath = true;
        currentHealth = 0;
        OnPlayerDie?.Invoke();
        animator.SetTrigger(_dieHash);
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (!other.gameObject.TryGetComponent(out EnemyState enemy)) return;

        TakeDamage(enemy.DamageByContact, other.transform);
    }

    private IEnumerator DamageRecoveryRoutine()
    {
        yield return new WaitForSeconds(invulnerabilityRecoveryTime);
        canTakeDamage = true;
    }
}