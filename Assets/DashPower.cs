using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashPower : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent(out Dash dash)) return;
        dash.enabled = true;
        Destroy(gameObject);
    }
}