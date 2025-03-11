
    using System;
    using System.Collections;
    using UnityEngine;

    public class Dash: MonoBehaviour
    {
        [SerializeField] private PlayerController playerController;
        [SerializeField] private float dashSpeed = 4f;
        [SerializeField] private TrailRenderer trailRenderer;
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
            if(Input.GetKeyDown(KeyCode.Space))
                DoDash();
        }

        private void DoDash()
        {
            if (!_isDashing /*&& Stamina.Instance.CurrentStamina > 0*/)
            {
                // Stamina.Instance.UseStamina();
                _isDashing = true;
                playerController.Speed *= dashSpeed;
                trailRenderer.emitting = true;
                StartCoroutine(EndDashRoutine());
                // Audio();
            }
        }
        private IEnumerator EndDashRoutine()
        {
            yield return new WaitForSeconds(dashTime);
            playerController.Speed = _startingMoveSpeed;
            trailRenderer.emitting = false;
            yield return new WaitForSeconds(dashCoolDown);
            _isDashing = false;
        }
    }
