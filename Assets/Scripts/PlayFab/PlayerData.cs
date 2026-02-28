using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerDataSaved", menuName = "PlayerData")]
public class PlayerData : ScriptableObject
{
    [Header("Player Data")]
    [SerializeField] private Vector2 checkPointPosition; 
    [SerializeField] private  int currentHealth;
    [SerializeField] private int currentGold; 
    [SerializeField] private bool dashPower; 
    [SerializeField] private bool hasKey;
    
    public Vector2 CheckPointPosition { get => checkPointPosition; set => checkPointPosition = value; }

    public int CurrentHealth { get => currentHealth; set => currentHealth = value; }

    public int CurrentGold { get => currentGold; set => currentGold = value; }

    public bool DashPower { get => dashPower; set => dashPower = value; }

    public bool HasKey { get => hasKey; set => hasKey = value; }


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