using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashPower : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        var dash = other.gameObject.GetComponent<Dash>();
        if (dash == null) return;
        dash.enabled = true;
        Destroy(gameObject);
    }
}