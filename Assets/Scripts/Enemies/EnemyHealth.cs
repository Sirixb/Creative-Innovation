using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private Animator animator;
    [SerializeField] private int currentHealth = 100;
    [SerializeField] private bool canTakeDamage = true;
    [SerializeField] private float invulnerabilityRecoveryTime = 1f;
    public bool isDeath = false;

    public event Action OnEnemyDie;

    private readonly int _dieHash = Animator.StringToHash("die");

    public void TakeDamage(int damageAmount, Transform hitTransform)
    {
        if (isDeath || !canTakeDamage)
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
        if (currentHealth > 0 || isDeath) return;
        isDeath = true;
        currentHealth = 0;
        OnEnemyDie?.Invoke();
    }

    private IEnumerator DamageRecoveryRoutine()
    {
        yield return new WaitForSeconds(invulnerabilityRecoveryTime);
        canTakeDamage = true;
    }
}