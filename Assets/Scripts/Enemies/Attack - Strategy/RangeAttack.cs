using UnityEngine;

public class RangeAttack : AttackStrategy
{
    public override void Attack(Transform attacker, Transform target)
    {
        //Frame Rate
        //Instanciar flecha
        if (Vector2.Distance(attacker.position, target.position) <= attackRange)
        {
            Debug.Log($"🏹 Disparo de flecha realizado: {damage} de daño.");
            // Aquí puedes instanciar un proyectil
        }
    }
}