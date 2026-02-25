using UnityEngine;

public abstract class ConsumableEffect : ScriptableObject
{
    public string consumableName;
    [SerializeField] protected PlayerData playerData;
    public abstract void ApplyEffect(PlayerHealth playerHealth);
}