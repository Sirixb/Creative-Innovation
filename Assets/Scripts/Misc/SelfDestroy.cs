using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    [SerializeField] private float timeToDestroy = 0f;

    public void DestroySelfAnimationEvent()
    {
        Destroy(gameObject, timeToDestroy);
    }
}