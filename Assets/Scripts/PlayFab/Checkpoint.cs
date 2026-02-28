using System;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private bool guardarEnCheckpoint = true;
    [SerializeField] private bool resetDataPlayer = false;
    private IPlayerDataManager _playerDataManager;

    private void Start()
    {
        _playerDataManager = ServiceLocator.Get<IPlayerDataManager>();
    }

    /// <summary>
    /// Detecta cuando el jugador toca el checkpoint.
    /// </summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        Debug.Log("[Checkpoint] Player collision");

        if (resetDataPlayer)
        {
            _playerDataManager.PlayerData.ResetData();
        }
        else
        {
            _playerDataManager.PlayerData.CheckPointPosition = transform.position;
        }

        if (_playerDataManager != null && guardarEnCheckpoint)
        {
            _playerDataManager.GuardarDatosDelJugador();
            Debug.Log("[Checkpoint] Checkpoint alcanzado - Datos guardados.");
        }
    }
}