using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class EnemyHealth : Health
{
    [SerializeField] private GameObject deathZombieVFXPrefab;
    [SerializeField] private DropConsumable dropConsumable;
    
    [Header("Damage ")]
    [SerializeField] private int damageByContact = 10;
    public int DamageByContact => damageByContact;
   
    public event Action OnEnemyDie;

    private readonly int _dieHash = Animator.StringToHash("die");
    
    protected override void CheckIfDeath()
    {
        base.CheckIfDeath();
        
        if (!IsDeath) return;
        OnEnemyDie?.Invoke();

    }
     
    public void DeathEnemyVFX()
    {
        Instantiate(deathZombieVFXPrefab, transform.position, Quaternion.identity);
        ServiceLocator.Get<AudioController>().PlaySFX(clip);
        dropConsumable.DropItems();
        Destroy(gameObject);
    }
}
