﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] private UIFade uiFade;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private EnemyHealth bossEnemyHealth;
    [SerializeField] private float waitToLoadTime = 1f;

    public void Start()
    {
        playerHealth = GameObject.FindWithTag("Player").GetComponent<PlayerHealth>();
        playerHealth.OnPlayerDie += GameOver;
        bossEnemyHealth = GameObject.Find("Orc Boss(Clone)").GetComponent<EnemyHealth>();
        bossEnemyHealth.OnEnemyDie += GameWin;
    }

    private void GameWin()
    {
        StartCoroutine(CheckPlayerDeathBeforeWin());
    }

    private IEnumerator CheckPlayerDeathBeforeWin()
    {
        yield return new WaitForSeconds(1.5f);

        if (playerHealth.IsDeath)
        {
            yield break;
        }

        uiFade.GameWinCanvasGroup.gameObject.SetActive(true);
        uiFade.FadeToPanel(uiFade.GameWinCanvasGroup, targetAlpha: 1);
        playerHealth.gameObject.GetComponent<PlayerController>().DisableComponentsOnPlayerDie();
    }

    private void GameOver()
    {
        uiFade.GameOverCanvasGroup.gameObject.SetActive(true);
        uiFade.FadeToPanel(uiFade.GameOverCanvasGroup, targetAlpha: 1);
    }

    public void Restart()
    {
        FadeOffPanel();
        StartCoroutine(LoadSceneRoutine(sceneToLoad: 1));
    }

    public void MainMenu()
    {
        FadeOffPanel();
        StartCoroutine(LoadSceneRoutine(sceneToLoad: 0));
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
        playerHealth.OnPlayerDie -= GameOver;
        bossEnemyHealth.OnEnemyDie -= GameWin;
    }
}