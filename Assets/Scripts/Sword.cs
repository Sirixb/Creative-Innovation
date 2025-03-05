using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class Sword : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Player player;
    [SerializeField] private Transform activeWeapon;

    [SerializeField] private GameObject slashAnimPrefab;
    [SerializeField] private Transform slashAnimSpawnPoint;
    [SerializeField] private Collider2D weaponCollider;

    private Camera _mainCamera;
    private GameObject _slash;

    private readonly int _attackHash = Animator.StringToHash("Attack");

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        MouseFollowWithOffset();
        Attack();
    }

    private void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger(_attackHash);
            _slash = Instantiate(slashAnimPrefab, slashAnimSpawnPoint.position, player.transform.rotation);
            _slash.transform.parent = this.transform.parent;
            weaponCollider.gameObject.SetActive(true);
        }
    }
    private void SwingUpFlipAnimEvent()
    {
        _slash.transform.localScale = new Vector3(1.5f, -1.5f, 1);
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
}