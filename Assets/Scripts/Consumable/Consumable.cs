using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Consumable : MonoBehaviour
{
    [SerializeField] private ConsumableEffect effect;
    [SerializeField] private float pickUpDistance = 4f;
    [SerializeField] private float accelartionRate = .2f;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private AnimationCurve animCurve;
    [SerializeField] private float heightY = 1.5f;
    [SerializeField] private float popDuration = .5f;
    [SerializeField] private AudioClip[] audioClips;


    private Vector3 _moveDirection;
    private Rigidbody2D _rigidbody2d;
    private AudioSource _audioSource;
    private Collider2D _collider2d;
    private SpriteRenderer _spriteRenderer;
    private PlayerHealth _playerHealth;


    private void Awake()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _collider2d = GetComponent<Collider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        StartCoroutine(AnimCurveSpawnRoutine());
    }

    private void Update()
    {
        var playerPos = _playerHealth.transform.position;

        if (Vector3.Distance(transform.position, playerPos) < pickUpDistance)
        {
            _moveDirection = (playerPos - transform.position).normalized;
            moveSpeed += accelartionRate;
        }
        else
        {
            _moveDirection = Vector3.zero;
            moveSpeed = 0f;
        }
    }

    private void FixedUpdate()
    {
        _rigidbody2d.velocity = _moveDirection * (moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent(out PlayerHealth player)) return;
        effect.ApplyEffect(player);
        _collider2d.enabled = false;
        _spriteRenderer.enabled = false;
        Audio();
        Destroy(gameObject, .5f);
    }

    private IEnumerator AnimCurveSpawnRoutine()
    {
        Vector2 startPoint = transform.position;
        var endPoint = startPoint + Random.insideUnitCircle;
        var timePassed = 0f;

        while (timePassed < popDuration)
        {
            timePassed += Time.deltaTime;
            var linearT = timePassed / popDuration;
            var heightT = animCurve.Evaluate(linearT);
            var height = Mathf.Lerp(0f, heightY, heightT);

            transform.position = Vector2.Lerp(startPoint, endPoint, linearT) + new Vector2(0f, height);

            yield return null;
        }
    }

    private void Audio()
    {
        int audioClip = Random.Range(0, audioClips.Length);
        _audioSource.PlayOneShot(audioClips[audioClip], .9f);
        // AudioSource.PlayClipAtPoint(audioClips[audioClip], transform.position,1f);
    }
}