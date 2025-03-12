using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIFade : MonoBehaviour

{
    [SerializeField] private Image fadeScreen;
    [SerializeField] private float fadeSpeed = 1;
    [SerializeField] private CanvasGroup gameOverCanvasGroup;
    [SerializeField] private CanvasGroup gameWinCanvasGroup;

    private IEnumerator fadeRoutine;

    public CanvasGroup GameOverCanvasGroup => gameOverCanvasGroup;

    public CanvasGroup GameWinCanvasGroup => gameWinCanvasGroup;

    public void Start()
    {
        DisablePanels();
    }

    private void DisablePanels()
    {
        gameOverCanvasGroup.alpha = 0;
        gameWinCanvasGroup.alpha= 0;
        GameOverCanvasGroup.interactable = false;
        GameWinCanvasGroup.interactable = false;
        gameOverCanvasGroup.gameObject.SetActive(false);
        GameWinCanvasGroup.gameObject.SetActive(false);
    }

    public void FadeToBlack()
    {
        if (fadeRoutine != null)
        {
            StopCoroutine(fadeRoutine);
        }

        fadeRoutine = FadeRoutine(1f);
        StartCoroutine(fadeRoutine);
    }

    public void FadeToClear()
    {
        if (fadeRoutine != null)
        {
            StopCoroutine(fadeRoutine);
        }

        fadeRoutine = FadeRoutine(0f);
        StartCoroutine(fadeRoutine);
    }

    private IEnumerator FadeRoutine(float targetAlpha)
    {
        while (!Mathf.Approximately(fadeScreen.color.a, targetAlpha))
        {
            var alpha = Mathf.MoveTowards(fadeScreen.color.a, targetAlpha, fadeSpeed * Time.deltaTime);
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, alpha);
            yield return null;
        }
    }

    public void FadeToPanel(CanvasGroup canvasGroup, float targetAlpha)
    {
        StartCoroutine(FadeRoutineCanvas(canvasGroup, targetAlpha));
    }


    private IEnumerator FadeRoutineCanvas(CanvasGroup canvasGroup, float targetAlpha)
    {
        while (!Mathf.Approximately(canvasGroup.alpha, targetAlpha))
        {
            var alpha = Mathf.MoveTowards(canvasGroup.alpha, targetAlpha, fadeSpeed * Time.deltaTime);
            canvasGroup.alpha =  alpha;
            canvasGroup.interactable = alpha >= 1f;
            yield return null;
        }
    }
}