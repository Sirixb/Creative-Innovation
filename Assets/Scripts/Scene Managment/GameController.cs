using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
    [SerializeField] private UIFade uiFade;
    [SerializeField] private float waitToLoadTime = 1f;
    
    private PlayerHealth _playerHealth;
    private EnemyHealth _bossEnemyHealth;

    [SerializeField] private Transform chestContainer;
    [SerializeField] private List<DropConsumable> chests;

    [SerializeField] private AudioClip introMusic;
    public void Start()
    {
        _playerHealth = GameObject.FindWithTag("Player").GetComponent<PlayerHealth>();
        _playerHealth.OnPlayerDie += GameOver;
        _bossEnemyHealth = GameObject.Find("Orc Boss(Clone)").GetComponent<EnemyHealth>();
        _bossEnemyHealth.OnEnemyDie += GameWin;
        
        chests = chestContainer.gameObject.GetComponentsInChildren<DropConsumable>().ToList();
        
        var chestSelectTokey = Random.Range(0, chests.Count);
        chests[chestSelectTokey].HasKey = true;
        
        var audioManager = ServiceLocator.Get<AudioController>();
        audioManager?.PlayMusic(introMusic);
    }

    private void GameWin()
    {
        StartCoroutine(CheckPlayerDeathBeforeWin());
    }

    private IEnumerator CheckPlayerDeathBeforeWin()
    {
        yield return new WaitForSeconds(1.5f);

        if (_playerHealth.IsDeath)
        {
            yield break;
        }

        uiFade.GameWinCanvasGroup.gameObject.SetActive(true);
        uiFade.FadeToPanel(uiFade.GameWinCanvasGroup, targetAlpha: 1);
        _playerHealth.gameObject.GetComponent<PlayerController>().DisableComponentsOnPlayerDie();
    }

    private void GameOver()
    {
        uiFade.GameOverCanvasGroup.gameObject.SetActive(true);
        uiFade.FadeToPanel(uiFade.GameOverCanvasGroup, targetAlpha: 1);
    }

    public void Restart()
    {
        FadeOffPanel();
        StartCoroutine(LoadSceneRoutine(sceneToLoad: 0));
    }

    public void Quit()
    {
        FadeOffPanel();
        Application.Quit();
    }

    private void FadeOffPanel()
    {
        uiFade.GameWinCanvasGroup.interactable = false;
        uiFade.GameOverCanvasGroup.interactable = false;
        uiFade.FadeToPanel(uiFade.GameWinCanvasGroup, targetAlpha: 0);
        uiFade.FadeToPanel(uiFade.GameOverCanvasGroup, targetAlpha: 0);
    }

    private IEnumerator LoadSceneRoutine(int sceneToLoad)
    {
        yield return new WaitForSeconds(waitToLoadTime);
        SceneManager.LoadScene(sceneToLoad);
    }

    private void OnDisable()
    {
        _playerHealth.OnPlayerDie -= GameOver;
        _bossEnemyHealth.OnEnemyDie -= GameWin;
    }
}