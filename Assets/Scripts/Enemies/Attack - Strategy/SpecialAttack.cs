using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
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

    [SerializeField] private GameObject magicBornPrefab;
    [SerializeField] private int maxEnemiesSpawned = 6;
    [SerializeField] private List<SpawnData> spawnList;
    [SerializeField] private List<GameObject> enemySpawnedList;
    private int damageByBossDie = 300;

    private void OnEnable()
    {
        enemyHealth.OnEnemyDie += DestroySpawnedEnemies;
    }

    public override void Attack(Transform attacker, Transform target)
    {
        this._target = target;
        var randomValue = Random.value;
        if (randomValue >= 0.9f && LivingEnemies() < maxEnemiesSpawned - 1)
        {
            CreateOrcs();
            return;
        }

        const int attackBasedOnRangePredefined = 5;
        if (attackRange > attackBasedOnRangePredefined)
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
        ChangeAttackAtributtes(damage: 15, range: 6, rate: 8);
        var spawner = FindObjectOfType<Spawner>();
        foreach (var spawnData in spawnList)
        {
            var position = (Vector2)transform.position;
            var insideUnitCircle = Random.insideUnitCircle;
            var enemySpawned = spawner.Spawn(spawnData.characterId, position + insideUnitCircle);
            Instantiate(magicBornPrefab, position + insideUnitCircle, Quaternion.identity);
            enemySpawnedList.Add(enemySpawned);
        }
    }

    private void RangeAttack()
    {
        ChangeAttackAtributtes(damage: 15, range: 1.3f, rate: 2f);
        Vector2 direction = (_target.position - firePoint.position).normalized;
        var lance = Instantiate(arrowPrefab, firePoint.position, Quaternion.identity);
        lance.GetComponent<Lance>().Damage = Damage;
        lance.GetComponent<Rigidbody2D>().velocity = direction * arrowSpeed;

        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        lance.transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
    }

    private void MeleeAttack()
    {
        ChangeAttackAtributtes(damage: 30, range: 10f, rate: 1.2f);
        collider2d.enabled = true;
        StartCoroutine(FinishMeleeAttack());
    }

    private IEnumerator FinishMeleeAttack()
    {
        yield return new WaitForSeconds(timeToFinishMeleeAttack);
        collider2d.enabled = false;
    }

    private void ChangeAttackAtributtes(int damage, float range, float rate)
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

    private void DestroySpawnedEnemies()
    {
        for (var i = enemySpawnedList.Count - 1; i >= 0; i--)
        {
            if (enemySpawnedList[i] == null)
            {
                enemySpawnedList.RemoveAt(i);
            }

            if (enemySpawnedList[i] != null)
            {
                enemySpawnedList[i].GetComponent<EnemyHealth>()?.TakeDamage(damageByBossDie, transform);
            }
        }
    }

    private int LivingEnemies()
    {
        for (var i = 0; i < enemySpawnedList.Count; i++)
        {
            if (enemySpawnedList[i] == null)
            {
                enemySpawnedList.RemoveAt(i);
            }
        }

        return enemySpawnedList.Count;
    }

    private void OnDisable()
    {
        enemyHealth.OnEnemyDie -= DestroySpawnedEnemies;
    }
}