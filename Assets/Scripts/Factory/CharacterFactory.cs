using System.Collections.Generic;
using UnityEngine;

public class CharacterFactory : MonoBehaviour
{
    [SerializeField] private CharacterData[] characterDatas;
    private Dictionary<string, CharacterData> _characterDictionary;

    private void Awake()
    {
        _characterDictionary = new Dictionary<string, CharacterData>();
        foreach (var data in characterDatas)
        {
            if (!_characterDictionary.ContainsKey(data.characterID))
            {
                _characterDictionary[data.characterID] = data;
            }
            else
            {
                Debug.LogWarning($"ID Duplicado detectado: {data.characterID}");
            }
        }
    }

    public GameObject CreateCharacter(string id, Vector3 position)
    {
        if (_characterDictionary.TryGetValue(id, out var characterData))
        {
            var instance = Instantiate(characterData.prefab, position, Quaternion.identity);
            return instance;
        }

        Debug.LogError($"ID {id} no encontrado en la Factory.");
        return null;
    }
}