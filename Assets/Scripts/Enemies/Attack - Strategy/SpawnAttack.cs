using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnAttack : AttackStrategy
{
    [Serializable]
    private struct SpawnData
    {
        public string characterId;
        public Transform spawnPosition;
    }

    [SerializeField] private EnemyHealth enemyHealth;
    [SerializeField] private GameObject magicBornPrefab;
    [SerializeField] private int maxEnemiesSpawned = 6;
    [SerializeField] private List<SpawnData> spawnList;
    [SerializeField] private List<GameObject> enemySpawnedList;
    private const int DamageByBossDie = 300;

    public int MaxEnemiesSpawned => maxEnemiesSpawned;

    public List<GameObject> EnemySpawnedList => enemySpawnedList;

    private void OnEnable()
    {
        enemyHealth.OnEnemyDie += DestroySpawnedEnemies;
    }

    public override void Attack(Transform attacker, Transform target)
    {
        CreateOrcs(attacker);
    }

    private void CreateOrcs(Transform attacker)
    {
        var spawner = FindObjectOfType<Spawner>();
        var position = (Vector2)attacker.position;
        foreach (var spawnData in spawnList)
        {
            var insideUnitCircle = Random.insideUnitCircle;
            var enemySpawned = spawner.Spawn(spawnData.characterId, position + insideUnitCircle);
            EnemySpawnedList.Add(enemySpawned);
            Instantiate(magicBornPrefab, position + insideUnitCircle, Quaternion.identity);
        }
    }
    private void DestroySpawnedEnemies()
    {
        EnemySpawnedList.RemoveAll(e => e == null);

        foreach (var enemy in EnemySpawnedList)
        {
            enemy.GetComponent<EnemyHealth>()?.TakeDamage(DamageByBossDie, transform);
        }
    }

    public int LivingEnemies()
    {
        enemySpawnedList.RemoveAll(e => e is null);
        return EnemySpawnedList.Count;
    }

    private void OnDisable()
    {
        enemyHealth.OnEnemyDie -= DestroySpawnedEnemies;
    }
}