using System;
using UnityEngine;

public class MeleeAttack : AttackStrategy
{
    public override void Attack(Transform attacker, Transform target)
    {
        //Hacer ataque: activar collider
        Debug.Log($"Ataque cuerpo a cuerpo realizado: {damage} de daño.");
    }
}