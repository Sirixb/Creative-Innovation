using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private float speed;
    [SerializeField] private Vector2 movement;
    [SerializeField] private bool facingLeft;

    private readonly int _runHash = Animator.StringToHash("run");
    private Camera _mainCamera;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Update()
    {
        FacingDirection();
        Animation();
    }

    private void Move()
    {
        movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        movement.Normalize();
        rb.MovePosition(rb.position + movement * (speed * Time.deltaTime));
    }

    private void FacingDirection()
    {
        var mousePosition = Input.mousePosition;
        var playerScreenPoint = _mainCamera.WorldToScreenPoint(transform.position);

        if (mousePosition.x < playerScreenPoint.x)
        {
            spriteRenderer.flipX = true;
            facingLeft = true;
        }
        else
        {
            spriteRenderer.flipX = false;
            facingLeft = false;
        }
    }

    private void Animation()
    {
        var move = Math.Abs(movement.x) + Math.Abs(movement.y);
        animator.SetFloat(_runHash, move);
    }
}