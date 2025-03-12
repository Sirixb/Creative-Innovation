using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpecialAttack : AttackStrategy
{
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Collider2D collider2d;
    [SerializeField] private EnemyHealth enemyHealth;
    [SerializeField] private float arrowSpeed = 5f;
    [SerializeField] private float timeToFinishMeleeAttack = .5f;
    private Transform _target;

    [Serializable]
    private struct SpawnData
    {
        public string characterId;
        public Transform spawnPosition;
    }

    [SerializeField] private List<SpawnData> spawnList;
    [SerializeField] private List<GameObject> enemySpawnedList;

    private void OnEnable()
    {
        enemyHealth.OnEnemyDie += DestroySpawnedEnemies;
    }

    private void DestroySpawnedEnemies()
    {
        foreach (var enemy in enemySpawnedList)
        {
            var damageByBossDie = 300;
            enemy.GetComponent<EnemyHealth>()?.TakeDamage(damageByBossDie,transform);
        }
        
        enemySpawnedList.RemoveAll(e => e == null);
    }

    public override void Attack(Transform attacker, Transform target)
    {
        this._target = target;
        var randomValue = Random.value;
        if (randomValue >= 0.9f)
        {
            CreateOrcs();
            return;
        }

        if (attackRange > 10)
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
        ChangeAttackAtributtes(damage: 15, rate: 8, range: 6);
        var spawner = FindObjectOfType<Spawner>();
        foreach (var spawnData in spawnList)
        {
            var enemySpawned = spawner.Spawn(spawnData.characterId, (Vector2)transform.position + Random.insideUnitCircle);
            enemySpawnedList.Add(enemySpawned);
        }
    }

    private void RangeAttack()
    {
        ChangeAttackAtributtes(damage: 15, rate: 2f, range: 1.3f);
        Vector2 direction = (_target.position - firePoint.position).normalized;
        var lance = Instantiate(arrowPrefab, firePoint.position, Quaternion.identity);
        lance.GetComponent<Lance>().Damage = Damage;
        lance.GetComponent<Rigidbody2D>().velocity = direction * arrowSpeed;

        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        lance.transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
    }

    private void MeleeAttack()
    {
        ChangeAttackAtributtes(damage: 30, rate: 1.2f, range: 13f);
        collider2d.enabled = true;
        StartCoroutine(FinishMeleeAttack());
    }

    private IEnumerator FinishMeleeAttack()
    {
        yield return new WaitForSeconds(timeToFinishMeleeAttack);
        collider2d.enabled = false;
    }

    private void ChangeAttackAtributtes(int damage, float rate, float range)
    {
        this.damage = damage;
        attackRate = rate;
        attackRange = range;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var playerHealth = other.gameObject.GetComponent<PlayerHealth>();
        playerHealth?.TakeDamage(damage, transform);
    }
    private void OnDisable()
    {
        enemyHealth.OnEnemyDie -= DestroySpawnedEnemies;
    }
}