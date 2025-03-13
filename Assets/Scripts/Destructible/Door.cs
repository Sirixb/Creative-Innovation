using System;
using System.Collections;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Door : MonoBehaviour
{
    [SerializeField] private GameObject keyedLock;
    [SerializeField] private GameObject particles;
    [SerializeField] private GameObject door;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float targetXMoveDoor = 5f;
    [SerializeField] private BoxCollider2D boxCollider2D;
    [SerializeField] private CanvasGroup findKeyCanvasGroup;
    [SerializeField] private float fadeSpeed = 1f;
    private float _initialPosition;
    [SerializeField] private AudioClip finalMusic;

    private void Start()
    {
        _initialPosition = transform.position.x;
        StartCoroutine(FindKey(findKeyCanvasGroup, targetAlpha: 0));
    }

    private void PutKey()
    {
        if (keyedLock == null || particles == null) return;
        Instantiate(particles, transform.position, Quaternion.identity);
        Instantiate(keyedLock, transform.position, Quaternion.identity);
        StartCoroutine(OpenDoor());
    }

    private IEnumerator OpenDoor()
    {
        var doorPosition = door.transform.position.x;
        while (door.transform.position.x > _initialPosition - targetXMoveDoor)
        {
            door.transform.Translate(Vector2.left * (speed * Time.deltaTime));
            yield return null;
        }

        Debug.Log("Open door");
        boxCollider2D.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var playerHealth = other.GetComponent<PlayerHealth>();
        if (playerHealth != null && playerHealth.HasKey)
        {
            findKeyCanvasGroup.alpha = 0;
            PutKey();
            ServiceLocator.Get<AudioController>().PlayMusic(finalMusic);
        }
        else
        {
            StartCoroutine(FindKey(findKeyCanvasGroup, targetAlpha: 1f));
        }
    }

    private IEnumerator FindKey(CanvasGroup canvasGroup, float targetAlpha)
    {
        while (!Mathf.Approximately(canvasGroup.alpha, targetAlpha))
        {
            var alpha = Mathf.MoveTowards(canvasGroup.alpha, targetAlpha, fadeSpeed * Time.deltaTime);
            canvasGroup.alpha = alpha;
            yield return null;
        }
        canvasGroup.alpha = targetAlpha;
    }
    

    private void OnTriggerExit2D(Collider2D other)
    {
        var playerHealth = other.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            StartCoroutine(FindKey(findKeyCanvasGroup, targetAlpha: 0));
        }
    }
}