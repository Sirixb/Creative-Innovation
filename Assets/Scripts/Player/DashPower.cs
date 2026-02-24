using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashPower : MonoBehaviour
{
    [SerializeField] private AudioClip clip;

    private void Start()
    {
        // Verificar si el jugador ya  tiene el poder de dash desde PlayFab
        var dash = FindObjectOfType<Dash>();
        if (dash != null && dash.enabled)
        {
            Debug.Log("[DashPower] El jugador ya tiene dash. Destruyendo objeto. ");
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent(out Dash dash)) return;
        dash.enabled = true;
        Destroy(gameObject);
        ServiceLocator.Get<AudioController>().PlaySFX(clip);
    }
}