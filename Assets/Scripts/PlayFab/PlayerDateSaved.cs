using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerDataSaved", menuName = "PlayerData")]
public class PlayerDataSaved : ScriptableObject
{
    public Vector2 checkPointPosition; 
    public int currentHealth; 
    public int currentGold; 
    public bool dashPower; 
    public bool hasKey; 
}