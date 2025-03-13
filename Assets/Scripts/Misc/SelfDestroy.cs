using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    [SerializeField] private float timeToDestroy = 1f;
    [SerializeField] private bool destroyByAnimationEvent = false;
    private ParticleSystem _particleSystem;

    private void Awake()
    {
        _particleSystem ??= GetComponent<ParticleSystem>();
    }

    private void Start()
    {
        if (!destroyByAnimationEvent)
            Destroy(gameObject, timeToDestroy);
    }

    private void Update()
    {
        // if (_particleSystem && !_particleSystem.IsAlive() && destroyByAnimationEvent)
        //     DestroySelfAnimationEvent();
    }

    public void DestroySelfAnimationEvent()
    {
        Destroy(gameObject);
    }
}