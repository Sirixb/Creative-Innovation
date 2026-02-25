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
    [SerializeField] private bool hasKey = false;
    [SerializeField] private bool dashPower = false;

    private const string HealthSliderText = "Health Slider";
    private const string CoinAmountText = "Gold Amount Text";
    private readonly int _dieHash = Animator.StringToHash("die");
    
    [Header("PlayFab")]
    [SerializeField] private PlayerDataManager playerDataManager;
    [Header("Player Data")]
    [SerializeField] private PlayerData playerData;
    public bool HasKey /*{ get*/ => hasKey; /*set => hasKey = value; }*/
    public int CurrentGold => currentGold;
    public int CurrentHealth => currentHealth;
    public bool DashPower => dashPower;
    [SerializeField] private Dash dash;
   
    
    public void Start()
    {
        LoadPlayerData();//Se llama aqui para usar PlayerData SO sin Playfab
        
        playerDataManager = FindObjectOfType<PlayerDataManager>();
        // if(playerDataManager != null)
        //     playerDataManager.OnDatosCargados += LoadPlayerData;//con Playfab
        
        _healthSlider ??= GameObject.Find(HealthSliderText)?.GetComponent<Slider>();
        _goldText ??= GameObject.Find(CoinAmountText)?.GetComponent<TMP_Text>();
        UpdateHealthSlider();
        UpdateCurrencyUI();
    }

    private void LoadPlayerData()
    {
        transform.position = playerData.checkPointPosition;
        currentHealth = playerData.currentHealth;
        currentGold = playerData.currentGold;
        hasKey = playerData.hasKey;
        dashPower = playerData.dashPower;
        dash.enabled = playerData.dashPower;
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
        playerData.currentHealth = currentHealth;
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
        if(playerDataManager)
            playerDataManager.OnDatosCargados -= LoadPlayerData;
    }
}