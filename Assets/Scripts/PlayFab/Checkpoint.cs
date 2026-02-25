using System;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private PlayerDataManager playerDataManager;
    [SerializeField] private PlayerData playerData;

    private void Start()
    {
        playerDataManager = FindObjectOfType<PlayerDataManager>();
    }

    /// <summary>
    /// Detecta cuando el jugador toca el checkpoint.
    /// </summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        Debug.Log("[Checkpoint] Player collision");
        playerData.checkPointPosition = transform.position;

        if (playerDataManager && playerDataManager.guardarEnCheckpoint)
        {
            playerDataManager.GuardarDatosDelJugador();
            Debug.Log("[Checkpoint] Checkpoint alcanzado - Datos guardados.");
        }
    }
}