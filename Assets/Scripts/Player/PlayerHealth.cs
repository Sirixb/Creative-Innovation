using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : Health
{
    public event Action OnPlayerDie;

    private Slider _healthSlider;
    private TMP_Text _goldText;
    private int _currentGold = 0;

    private const string HealthSliderText = "Health Slider";
    private const string CoinAmountText = "Gold Amount Text";
    private readonly int _dieHash = Animator.StringToHash("die");
    [SerializeField] private bool hasKey = false;
    public bool HasKey { get => hasKey; set => hasKey = value; }

    public void Start()
    {
        _healthSlider ??= GameObject.Find(HealthSliderText)?.GetComponent<Slider>();
        _goldText ??= GameObject.Find(CoinAmountText)?.GetComponent<TMP_Text>();
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

    public override void RestoreHealth(int damageAmount)
    {
        base.RestoreHealth(damageAmount);
        UpdateHealthSlider();
    }

    private void UpdateHealthSlider()
    {
        _healthSlider.maxValue = maxHealth;
        _healthSlider.value = currentHealth;
    }

    public void UpdateCurrency(int gold)
    {
        _currentGold += gold;
        _goldText.text = _currentGold.ToString("D3");
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (!other.gameObject.TryGetComponent(out EnemyHealth enemy)) return;
        TakeDamage(enemy.DamageByContact, other.transform);
    }
}