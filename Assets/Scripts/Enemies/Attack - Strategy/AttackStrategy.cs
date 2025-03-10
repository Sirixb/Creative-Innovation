using System;
using Unity.VisualScripting;
using UnityEngine;

public abstract class AttackStrategy : MonoBehaviour
{
    [SerializeField] protected int damage = 10;
    [SerializeField] protected float attackRange = 2f;
    [SerializeField] protected float attackRate = 1f;

    public int Damage => damage;
    public float AttackRange => attackRange;
    public float AttackRate => attackRate;

    public abstract void Attack(Transform attacker, Transform target, Collider2D collider2d);
}