using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] private UIFade uiFade;
    [SerializeField] private PlayerHealth playerHealth;
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
        uiFade.FadeToPanel(uiFade.GameWinCanvasGroup, targetAlpha: 1);
    }

    private void GameOver()
    {
        uiFade.FadeToPanel(uiFade.GameOverCanvasGroup, targetAlpha: 1);
    }

    private void CleanPanel()
    {
        uiFade.FadeToPanel(uiFade.GameWinCanvasGroup, targetAlpha: 0);
        uiFade.FadeToPanel(uiFade.GameOverCanvasGroup, targetAlpha: 0);
    }
    public void Restart()
    {
        CleanPanel();
        StartCoroutine(LoadSceneRoutine(sceneToLoad:1));
    }

    public void MainMenu()
    {
        CleanPanel();
        StartCoroutine(LoadSceneRoutine(sceneToLoad:0));
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