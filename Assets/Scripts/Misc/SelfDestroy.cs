using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    public void DestroySelfAnimationEvent()
    {
        Destroy(gameObject);
    }
}
