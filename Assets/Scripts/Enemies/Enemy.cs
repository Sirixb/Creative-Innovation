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

    [Header("IA Configuration")]
    [SerializeField] private float minIdleTime = 1f;
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
    private AttackStrategy _attackStrategy;

    [Header("Vision Configuration")]
    [SerializeField] private Transform viewPosition;
    [SerializeField] private float viewDistance = 5f;
    [SerializeField] private ContactFilter2D contactFilter;
    [SerializeField] private bool isPlayerInSight;
    [SerializeField] private bool isPlayerInRange;
    // public bool IsPlayerInSight { get => _isPlayerInSight; private set => _isPlayerInSight = value; }
    // public bool IsPlayerInRange { get => _isPlayerInRange; private set => _isPlayerInRange = value; }

    private readonly List<RaycastHit2D> _results = new List<RaycastHit2D>();

    public int RunHash { get; } = Animator.StringToHash("run");
    public int AttackHash { get; } = Animator.StringToHash("attack");
    public int InRangeHash { get; } = Animator.StringToHash("inRange");

    private float _attackRateTimer = 0f;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        _circleCollider2D = GetComponentInChildren<CircleCollider2D>();
        _attackStrategy = GetComponentInChildren<AttackStrategy>();
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

    private Vector2 GetDirectionToPlayer()
    {
        var player = _player.transform.position;
        Vector2 target = player - viewPosition.position;
        target.Normalize();
        return target;
    }

    private Vector2 GetPlayerPosition()
    {
        return _player.transform.position;
    }

    public bool PlayerInSight()
    {
        isPlayerInSight = false;
        var target = GetDirectionToPlayer();
        Debug.DrawRay(viewPosition.position, target * viewDistance, Color.red);

        var hitCount = Physics2D.Raycast(viewPosition.position, target, contactFilter, _results, viewDistance);
        for (var i = 0; i < hitCount; i++)
        {
            // Debug.Log("Rayo detecto: " + _results[i].collider.name);
            if (_results[0].collider.CompareTag("Player"))
            {
                isPlayerInSight = true;
                break;
            }
        }

        return isPlayerInSight;
    }

    public bool PlayerInRange()
    {
        var enemy = transform.position;
        isPlayerInRange = Vector2.Distance(enemy, GetPlayerPosition()) < _attackStrategy.AttackRange;
        return isPlayerInRange;
    }

    public void ChasePlayer()
    {
        if (PlayerInRange())
        {
            SetPosition(Vector2.zero);
            return;
        }

        var target = GetDirectionToPlayer();
        Debug.DrawRay(transform.position, target, Color.green);
        SetPosition(target * (ChaseSpeed * Time.deltaTime));
        SetRotation(target.x);
    }

    public void SetPosition(Vector2 velocity)
    {
        _rb.MovePosition(_rb.position + velocity);
    }

    public Vector2 GetVelocity()
    {
        return _rb.velocity;
    }

    public void SetAnimation(string parameter)
    {
        _animator.Play(parameter, layer: 0, normalizedTime: 0);
    }

    public void SetRotation(float direction)
    {
        var localScale = direction >= 0 ? 1 : -1;
        transform.localScale = new Vector3(localScale, 1, 1);
    }

    public void Attack()
    {
        SetRotation(GetDirectionToPlayer().x);
        _attackStrategy?.Attack(transform, _player.transform);
    }

    public bool CanAttack()
    {
        _attackRateTimer += Time.deltaTime;
        if (_attackRateTimer >= _attackStrategy.AttackRate)
        {
            _attackRateTimer = 0f;
            return true;
        }

        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, GetComponentInChildren<AttackStrategy>().AttackRate);
    }
}