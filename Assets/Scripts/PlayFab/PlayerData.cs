using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerDataSaved", menuName = "PlayerData")]
public class PlayerData : ScriptableObject
{
    [Header("Player Data")]
    public Vector2 checkPointPosition; 
    public int currentHealth; 
    public int currentGold; 
    public bool dashPower; 
    public bool hasKey;
    
    [Header("Reset Data")]
    public Vector2 resetPosition;

    public void ResetData()
    {
        checkPointPosition = new Vector2(resetPosition.x, resetPosition.y);
        currentHealth = 100;
        currentGold = 0;
        dashPower = false;
        hasKey = false;
    }
}