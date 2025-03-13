using UnityEngine;

public abstract class ConsumableEffect : ScriptableObject
{
    public string consumableName;

    public abstract void ApplyEffect(PlayerHealth playerHealth);
}