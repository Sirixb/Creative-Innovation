using UnityEngine;

[CreateAssetMenu(fileName = "NewKeyConsumable", menuName = "Consumables/Key")]
public class KeyConsumable : ConsumableEffect
{
    public override void ApplyEffect(PlayerHealth player)
    {
        player.HasKey = true;
    }
}