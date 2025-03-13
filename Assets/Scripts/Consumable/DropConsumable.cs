using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropConsumable : MonoBehaviour
{
    [SerializeField] private GameObject goldCoin, healthGlobe, key;
    [SerializeField] private bool hasKey = false;

    public bool HasKey { get => hasKey; set => hasKey = value; }

    public void DropItems()
    {
        if (HasKey)
        {
            Instantiate(key, transform.position, Quaternion.identity);
            return;
        }
        
        var randomNum = Random.Range(1, 5);
        if (randomNum == 1)
        {
            Instantiate(healthGlobe, transform.position, Quaternion.identity);
        }
        else if (randomNum == 2)
        {
            Instantiate(goldCoin, transform.position, Quaternion.identity);
        }
        else if (randomNum == 3)
        {
            var randomAmountOfGold = Random.Range(1, 4);
            for (var i = 0; i < randomAmountOfGold; i++)
            {
                Instantiate(goldCoin, transform.position, Quaternion.identity);
            }
        }
        
    }
}