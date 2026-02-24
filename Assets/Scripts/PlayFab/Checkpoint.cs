using System;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private PlayerDataManager playerDataManager;

    private void Start()
    {
        playerDataManager= FindObjectOfType<PlayerDataManager>();
    }

    /// <summary>
    /// Detecta cuando el jugador toca el checkpoint.
    /// </summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        Debug.Log("[Checkpoint] Player collision");
        if (playerDataManager.guardarEnCheckpoint)
        {
            playerDataManager.GuardarDatosDelJugador(transform.position);
            Debug.Log("[Checkpoint] Checkpoint alcanzado - Datos guardados.");
        }
    }
}