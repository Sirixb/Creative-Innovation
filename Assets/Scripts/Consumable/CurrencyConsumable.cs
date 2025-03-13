using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCurrencyConsumable", menuName = "Consumables/Currency")]
public class CurrencyConsumable : ConsumableEffect
{
    [SerializeField] private int currencyAmount;

    public override void ApplyEffect(PlayerHealth playerHealth)
    {
        playerHealth.UpdateCurrency(currencyAmount);
    }
}