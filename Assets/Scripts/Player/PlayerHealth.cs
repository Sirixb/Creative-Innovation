using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerHealth : Health
{
    public event Action OnPlayerDie;

    //Refactor Inventory
    [Header("Inventory")]
    private Slider _healthSlider;
    private TMP_Text _goldText;
    [SerializeField] private int currentGold = 0;

    private const string HealthSliderText = "Health Slider";
    private const string CoinAmountText = "Gold Amount Text";
    private readonly int _dieHash = Animator.StringToHash("die");

    [Header("PlayFab and Player Data")]
    private IPlayerDataManager _playerDataManager;
    private PlayerData _playerData;


    public void Start()
    {
        _playerDataManager = ServiceLocator.Get<IPlayerDataManager>();
        if (_playerDataManager != null)
            _playerData = _playerDataManager.PlayerData;

        // LoadPlayerData();//Se llama aqui para usar PlayerData SO sin Playfab

        if (_playerDataManager != null)
            _playerDataManager.OnDatosCargados += LoadPlayerData; //con Playfab

        _healthSlider ??= GameObject.Find(HealthSliderText)?.GetComponent<Slider>();
        _goldText ??= GameObject.Find(CoinAmountText)?.GetComponent<TMP_Text>();
        UpdateHealthSlider();
        UpdateCurrencyUI();
    }

    private void LoadPlayerData(PlayerData playerData)
    {
        transform.position = playerData.CheckPointPosition;
        currentHealth = playerData.CurrentHealth;
        currentGold = playerData.CurrentGold;
        UpdateHealthSlider();
        UpdateCurrencyUI();
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
        _playerData.CurrentHealth = currentHealth;
    }

    public void UpdateCurrency(int gold)
    {
        currentGold += gold;
        UpdateCurrencyUI();
    }

    private void UpdateCurrencyUI()
    {
        _goldText.text = currentGold.ToString("D3");
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (!other.gameObject.TryGetComponent(out EnemyHealth enemy)) return;
        TakeDamage(enemy.DamageByContact, other.transform);
    }

    private void OnDestroy()
    {
        if (_playerDataManager != null)
            _playerDataManager.OnDatosCargados -= LoadPlayerData;
    }
}