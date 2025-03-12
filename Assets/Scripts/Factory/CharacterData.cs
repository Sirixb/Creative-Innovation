using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacterData", menuName = "Character/New Character")]
public class CharacterData : ScriptableObject
{
    public string characterID; 
    public GameObject prefab; 
}

