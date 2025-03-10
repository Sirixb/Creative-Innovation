using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBack : MonoBehaviour
{
	 
    [SerializeField] private float knockBackTime = .2f;
    [SerializeField] private Rigidbody2D rigidbody2d;
    public bool GettingKnockedBack{ get; private set; }

    public void GetKnockedBack(Transform damageSource, float knockBackThrust)
    {
        GettingKnockedBack = true;
        Vector2 difference = (transform.position - damageSource.position).normalized * knockBackThrust * rigidbody2d.mass;
        rigidbody2d.AddForce(difference, ForceMode2D.Impulse); 
        StartCoroutine(KnockRoutine());
    }
	
    private IEnumerator KnockRoutine()
    {
        yield return new WaitForSeconds(knockBackTime);
        rigidbody2d.velocity = Vector2.zero;
        GettingKnockedBack = false;
    }
}