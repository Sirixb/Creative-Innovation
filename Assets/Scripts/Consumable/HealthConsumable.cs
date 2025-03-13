using UnityEngine;

[CreateAssetMenu(fileName = "NewHealthConsumable", menuName = "Consumables/Health")]
public class HealthConsumable : ConsumableEffect
{
    public int healAmount;

    public override void ApplyEffect(PlayerHealth playerHealth)
    {
        playerHealth.RestoreHealth(healAmount);
    }
}