using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlayerController : Character
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private CapsuleCollider2D capsuleCollider2D;
    [SerializeField] private Collider2D collider2d;
    [SerializeField] private KnockBack knockBack;
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    private IWeapon _weapon;


    [SerializeField] private float speed;
    public float Speed { get => speed; set => speed = value; }
    public bool FacingLeft { get; private set; }


    private Vector2 _movement;

    private Camera _mainCamera;

    private readonly int _runHash = Animator.StringToHash("run");

    private void Awake()
    {
        _mainCamera = Camera.main;
        _weapon = GetComponentInChildren<IWeapon>();
         FindObjectOfType<CinemachineVirtualCamera>().m_Follow = transform;
    }
    private void OnEnable()
    {
        if (playerHealth != null)
        {
            playerHealth.OnPlayerDie += DisableComponentsOnPlayerDie;
        }
    }

    private void DisableComponentsOnPlayerDie()
    {
        _weapon.DisableWeapon();
        capsuleCollider2D.enabled = false;
        this.enabled = false;
    }

    private void FixedUpdate()
    {
        if (knockBack.GettingKnockedBack) { return; }
        Move();
    }

    private void Update()
    {
        FacingDirection();
        Animation();
    }

    private void Move()
    {
        _movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (_movement != Vector2.zero) _movement.Normalize();
        rb.MovePosition(rb.position + _movement * (Speed * Time.deltaTime));
    }

    private void FacingDirection()
    {
        var mousePosition = Input.mousePosition;
        var playerScreenPoint = _mainCamera.WorldToScreenPoint(transform.position);

        if (mousePosition.x < playerScreenPoint.x)
        {
            FacingLeft = true;
            transform.rotation = Quaternion.Euler(0, -180, 0);
        }
        else
        {
            FacingLeft = false;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    private void Animation()
    {
        var move = Math.Abs(_movement.x) + Math.Abs(_movement.y);
        animator.SetFloat(_runHash, move);
    }
    private void OnDisable()
    {
        playerHealth.OnPlayerDie -= DisableComponentsOnPlayerDie;
    }
}