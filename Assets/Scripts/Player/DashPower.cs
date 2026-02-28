using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DashPower : MonoBehaviour
{
    [SerializeField] private AudioClip clip;

    private IPlayerDataManager _playerDataManager;
    private PlayerData _playerData;

    private void OnEnable()
    {
        _playerDataManager = ServiceLocator.Get<IPlayerDataManager>();
        if (_playerDataManager != null)
            _playerDataManager.OnDatosCargados += LoadPlayerData;
    }

    private void LoadPlayerData(PlayerData playerData)
    {
        _playerData = playerData;
        if (playerData.DashPower)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent(out Dash dash)) return;
        dash.enabled = true;
        _playerData.DashPower = true;
        Destroy(gameObject);
        ServiceLocator.Get<AudioController>().PlaySFX(clip);
    }

    private void OnDisable()
    {
        if (_playerDataManager != null)
            _playerDataManager.OnDatosCargados -= LoadPlayerData;
    }
}