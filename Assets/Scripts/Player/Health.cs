using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Health : MonoBehaviour
{
    [SerializeField] protected Animator animator;
    [SerializeField] protected KnockBack knockBack;
    [SerializeField] protected Flash flash;
    [SerializeField] protected int maxHealth = 100;
    [SerializeField] protected int currentHealth = 100;
    [SerializeField] protected bool canTakeDamage = true;
    [SerializeField] protected float invulnerabilityRecoveryTime = 1f;
    [SerializeField] protected float knockBackThrustAmount = 10f;
    public bool IsDeath { get; private set; } = false;


    public virtual void TakeDamage(int damageAmount, Transform hitTransform)
    {
        if (IsDeath || !canTakeDamage)
        {
            return;
        }

        canTakeDamage = false;
        currentHealth -= damageAmount;
        knockBack.GetKnockedBack(hitTransform, knockBackThrustAmount);
        StartCoroutine(flash.FlashRoutine());
        StartCoroutine(DamageRecoveryRoutine());
        CheckIfDeath();
    }

    public virtual void RestoreHealth(int damageAmount)
    {
        if (IsDeath)
        {
            return;
        }
        currentHealth += damageAmount;
        if (currentHealth >= 100)
            currentHealth = 100;
    }

    protected virtual void CheckIfDeath()
    {
        if (currentHealth > 0 || IsDeath) return;
        IsDeath = true;
        currentHealth = 0;
    }

    private IEnumerator DamageRecoveryRoutine()
    {
        yield return new WaitForSeconds(invulnerabilityRecoveryTime);
        canTakeDamage = true;
    }
}