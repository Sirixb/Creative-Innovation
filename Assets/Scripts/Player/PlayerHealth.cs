using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : Health
{
    public event Action OnPlayerDie;
    private Slider _healthSlider;
    private const string HealthSliderText = "Health Slider";
    private readonly int _dieHash = Animator.StringToHash("die");

    public void Start()
    {
        UpdateHealthSlider();
    }

    public override void TakeDamage(int damageAmount, Transform hitTransform)
    {
        base.TakeDamage(damageAmount, hitTransform);
        UpdateHealthSlider();
    }

    protected override void CheckIfDeath()
    {
        base.CheckIfDeath();
        
        if (!IsDeath) return;
        OnPlayerDie?.Invoke();
        animator.SetTrigger(_dieHash);
    }
    private void UpdateHealthSlider()
    {
        if (_healthSlider == null)
        {
            _healthSlider = GameObject.Find(HealthSliderText).GetComponent<Slider>();
        }

        _healthSlider.maxValue = maxHealth;
        _healthSlider.value = currentHealth;
    }
    private void OnCollisionStay2D(Collision2D other)
    {
        if (!other.gameObject.TryGetComponent(out EnemyHealth enemy)) return;
        TakeDamage(enemy.DamageByContact, other.transform);
    }
}
