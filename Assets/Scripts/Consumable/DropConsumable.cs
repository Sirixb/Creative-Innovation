using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropConsumable : MonoBehaviour
{
    [SerializeField] private GameObject goldCoin, healthGlobe;
    [SerializeField] private AudioClip[] audioClips;
	
    public void DropItems()
    {
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
                Audio();
            }
        }
    }

    private void Audio()
    {
        var audioClip = Random.Range(0, audioClips.Length);
        // audioSource.PlayOneShot(audioClips[audioClip], 0.8f);
        // AudioSource.PlayClipAtPoint(audioClips[audioClip], transform.position, 1f);
    }
}
