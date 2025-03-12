using System;
using Unity.VisualScripting;
using UnityEngine;

public abstract class AttackStrategy : MonoBehaviour
{
    [SerializeField] protected int damage = 10;
    [SerializeField] protected float attackRange = 2f;
    [SerializeField] protected float attackRate = 1f;

    protected int Damage { get => damage; set => damage = value; }
    public float AttackRange { get => attackRange; set => attackRange = value; }
    public float AttackRate { get => attackRate; set => attackRate = value; }

    public abstract void Attack(Transform attacker, Transform target);
}