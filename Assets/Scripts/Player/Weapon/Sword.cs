using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour, IWeapon
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform activeWeapon;
    [SerializeField] private Collider2D weaponCollide;

    [SerializeField] private Player player;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Damager damager;

    [SerializeField] private Transform slashAnimSpawnPoint;
    [SerializeField] private GameObject slashAnimPrefab;

    [Header("Weapon Info")]
    [SerializeField] private int damage = 50;
    [SerializeField] private float attackRate = 1f;
    [SerializeField] private float attackRange = 2f;
    private bool IsAttack { get; set; } = false;

    private Camera _mainCamera;
    private GameObject _slash;

    private readonly int _attackHash = Animator.StringToHash("attack");
    private readonly int _dieHash = Animator.StringToHash("die");
    private float _attackCooldonwTimer = 0;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        damager = GetComponentInChildren<Damager>(true);
        damager.Config(damage);
        // player = GetComponent<Player>();
        // playerHealth = GetComponent<PlayerHealth>();
        // activeWeapon = GetComponent<Transform>();
        // weaponCollide = GetComponent<Collider2D>();
        _mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        if (playerHealth != null)
        {
            playerHealth.OnPlayerDie += AnimationWeaponOnPlayerDie;
        }
    }

    private void Update()
    {
        MouseFollowWithOffset();
        Attack();
    }

    private void Attack()
    {
        _attackCooldonwTimer += Time.deltaTime;
        if (Input.GetMouseButtonDown(0) && _attackCooldonwTimer > attackRate)
        {
            animator.SetTrigger(_attackHash);
            _slash = Instantiate(slashAnimPrefab, slashAnimSpawnPoint.position, player.transform.rotation);
            _slash.transform.parent = this.transform.parent;
            weaponCollide.gameObject.SetActive(true);
            _attackCooldonwTimer = 0;
        }
    }


    public void DoneAttackingAnimEvent()
    {
        weaponCollide.gameObject.SetActive(false);
    }

    private void SwingUpFlipAnimEvent()
    {
        _slash.transform.localScale = new Vector3(1.5f, -1.5f, 1);
    }

    private void AnimationWeaponOnPlayerDie()
    {
        animator.SetTrigger(_dieHash);
    }

    private void MouseFollowWithOffset()
    {
        var mousePosition = Input.mousePosition;
        var playerScreenPoint = _mainCamera.WorldToScreenPoint(player.transform.position);
        var playerPosition = player.transform.position;

        var angle = Mathf.Atan2(mousePosition.y - playerPosition.y, Mathf.Abs(mousePosition.x - playerPosition.x)) *
                    Mathf.Rad2Deg;

        if (mousePosition.x < playerScreenPoint.x)
        {
            activeWeapon.transform.rotation = Quaternion.Euler(0, -180, angle);
        }
        else
        {
            activeWeapon.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    public void DisableWeapon()
    {
        weaponCollide.gameObject.SetActive(false);
        this.enabled = false;
    }

    private void OnDisable()
    {
        playerHealth.OnPlayerDie -= AnimationWeaponOnPlayerDie;
    }
}