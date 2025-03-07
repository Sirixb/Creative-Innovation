using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D _rb;
    private CapsuleCollider2D _capsuleCollider2D;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private CircleCollider2D _circleCollider2D;
    private Player _player;

    [Header("IA Configuration")] [SerializeField]
    private float minIdleTime = 1f;

    [SerializeField] private float maxIdleTime = 3f;
    [SerializeField] private float patrolSpeed = 1f;
    [SerializeField] private float minPatrolTime = 1f;
    [SerializeField] private float maxPatrolTime = 3f;
    [SerializeField] private float chaseSpeed = 2f;

    public float MinIdleTime => minIdleTime;
    public float MaxIdleTime => maxIdleTime;
    public float ChaseSpeed => chaseSpeed;
    public float PatrolSpeed => patrolSpeed;
    public float MinPatrolTime => minPatrolTime;
    public float MaxPatrolTime => maxPatrolTime;

    private IEnemyState _currentState;
    private bool _isPlayerInRange;
    public int RunHash { get; } = Animator.StringToHash("run");
    public int AttackHash { get; } = Animator.StringToHash("attack");

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        _circleCollider2D = GetComponentInChildren<CircleCollider2D>();

        _player = FindObjectOfType<Player>();
    }

    private void Start()
    {
        ChangeState(new IdleState());
    }

    private void Update()
    {
        _currentState?.Update();
    }

    public void ChangeState(IEnemyState newState)
    {
        _currentState?.Exit();
        _currentState = newState;
        _currentState.Enter(this);
    }

    public bool CanSeePlayer()
    {
        return _isPlayerInRange;
    }

    public bool IsInAttackRange()
    {
        return _isPlayerInRange;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInRange = true;
            Debug.Log("player en range");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInRange = false;
            Debug.Log("Player out range");
        }
    }

    public void SetPosition(Vector2 velocity)
    {
        _rb.MovePosition(_rb.position + velocity);
    }

    public void SetAnimation(int parameter, float speed)
    {
        _animator.SetFloat(parameter, speed);
    }

    public void SetAnimation(int parameter)
    {
        _animator.SetTrigger(parameter);
    }

    public Vector3 GetPlayerPosition()
    {
        return _player.transform.position;
    }

    public void SetRotation(float direction)
    {
        var localScale = direction >= 0? 1:-1;
        transform.localScale = new Vector3(localScale, 1, 1);
    }

}