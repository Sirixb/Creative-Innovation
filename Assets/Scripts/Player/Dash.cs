using System;
using System.Collections;
using UnityEngine;

public class Dash : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private float dashSpeed = 4f;
    [SerializeField] private float dashTime = .2f;
    [SerializeField] private float dashCoolDown = .25f;

    private float _startingMoveSpeed;
    private bool _isDashing = false;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    private void Start()
    {
        _startingMoveSpeed = playerController.Speed;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            DoDash();
    }

    private void DoDash()
    {
        if (_isDashing) return;
        _isDashing = true;
        playerController.Speed *= dashSpeed;
        if (trailRenderer)
            trailRenderer.emitting = true;
        StartCoroutine(EndDashRoutine());
    }

    private IEnumerator EndDashRoutine()
    {
        yield return new WaitForSeconds(dashTime);
        playerController.Speed = _startingMoveSpeed;
        if (trailRenderer)
            trailRenderer.emitting = false;
        yield return new WaitForSeconds(dashCoolDown);
        _isDashing = false;
    }
}