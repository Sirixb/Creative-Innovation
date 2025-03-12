using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpecialAttack : AttackStrategy
{
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float arrowSpeed = 5f;
    [SerializeField] private Transform target;

    [Serializable]
    private struct SpawnData
    {
        public string characterId;
        public Transform spawnPosition;
    }

    [SerializeField] private List<SpawnData> spawnList;
    [SerializeField] private float timeToFinish = .5f;
    [SerializeField] private static Collider2D _collider2D;


    public override void Attack(Transform attacker, Transform target, Collider2D collider2d)
    {
        this.target = target;
        _collider2D = collider2d;
        var randomValue = Random.value;
        if (randomValue >= 0.9f)
        {
            CreateOrcs();
            return;
        }

        /*else*/
        if (attackRange > 10 /*randomValue is > .4f and < 0.9f*/)
        {
            RangeAttack();
        }
        else
        {
            MeleeAttack();
        }
    }

    private void CreateOrcs()
    {
        ChangeAttackAtributtes(damage: 15, rate: 8, range: 5);
        var spawner = FindObjectOfType<Spawner>();
        foreach (var spawnData in spawnList)
        {
            spawner.Spawn(spawnData.characterId, (Vector2)transform.position + Random.insideUnitCircle);
        }
    }

    private void RangeAttack()
    {
        ChangeAttackAtributtes(damage: 15, rate: 2f, range: 1);
        Vector2 direction = (target.position - firePoint.position).normalized;
        var lance = Instantiate(arrowPrefab, firePoint.position, Quaternion.identity);
        lance.GetComponent<Lance>().Damage = Damage;
        lance.GetComponent<Rigidbody2D>().velocity = direction * arrowSpeed;

        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        lance.transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
    }

    private void MeleeAttack()
    {
        ChangeAttackAtributtes(damage: 30, rate: 1.2f, range: 13f);
        _collider2D.enabled = true;
        StartCoroutine(FinishMeleeAttack());
    }

    private IEnumerator FinishMeleeAttack()
    {
        yield return new WaitForSeconds(timeToFinish);
        _collider2D.enabled = false;
    }

    private void ChangeAttackAtributtes(float damage, float rate, float range)
    {
        damage = damage;
        attackRate = rate;
        attackRange = range;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var playerHealth = other.gameObject.GetComponent<PlayerHealth>();
        playerHealth?.TakeDamage(damage, transform);
    }
}