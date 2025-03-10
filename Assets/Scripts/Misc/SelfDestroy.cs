using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    [SerializeField] private float timeToDestroy = 0f;
    private ParticleSystem _particleSystem;
    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (_particleSystem && !_particleSystem.IsAlive())
            DestroySelfAnimationEvent();
    }
    public void DestroySelfAnimationEvent()
    {
        Destroy(gameObject, timeToDestroy);
    }
}