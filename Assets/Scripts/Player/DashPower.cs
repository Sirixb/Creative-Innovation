using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DashPower : MonoBehaviour
{
    [SerializeField] private AudioClip clip;
    [SerializeField] private PlayerDataManager _playerDataManager;

    [SerializeField] private PlayerData playerData;

    private void OnEnable()
    {
        _playerDataManager.OnDatosCargados += DestroyPower;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent(out Dash dash)) return;
        dash.enabled = true;
        playerData.dashPower = true;
        Destroy(gameObject);
        ServiceLocator.Get<AudioController>().PlaySFX(clip);
    }

    private void DestroyPower()
    {
        if (playerData.dashPower)
            Destroy(gameObject);
    }

    private void OnDisable()
    {
        _playerDataManager.OnDatosCargados -= DestroyPower;
    }
}