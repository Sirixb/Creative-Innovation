using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private CharacterFactory characterFactory;

    [Serializable]
    private struct SpawnData
    {
        public string characterId;
        public Transform spawnPosition;
    }

    [SerializeField] private List<SpawnData> spawnList;

    private void Awake()
    {
        foreach (var spawnData in spawnList)
        {
            Spawn(spawnData.characterId, spawnData.spawnPosition.position);
        }
    }

    public GameObject Spawn(string spawnDataCharacterId, Vector3 spawnPositionPosition)
    {
        return characterFactory.CreateCharacter(spawnDataCharacterId, spawnPositionPosition);
    }
}