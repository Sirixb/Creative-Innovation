using System;
using Unity.VisualScripting;
using UnityEngine;

public abstract class AttackStrategy : MonoBehaviour
{
    [SerializeField] protected float damage = 10f;
    [SerializeField] protected float attackRange = 2f;
    [SerializeField] protected float attackRate = 1f;

    public float Damage => damage;
    public float AttackRange => attackRange;
    public float AttackRate => attackRate;

    public abstract void Attack(Transform attacker, Transform target);
}