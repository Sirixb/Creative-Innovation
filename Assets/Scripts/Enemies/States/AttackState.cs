using UnityEngine;

public class AttackState : IEnemyState
{
    private Enemy _enemy;

    public void Enter(Enemy enemy)
    {
        this._enemy = enemy;
        // Debug.Log("Entrando en estado de ataque");
        Attack();
    }

    public void Update()
    {
        _enemy.SetPosition(Vector2.zero);

        if (!_enemy.CanAttack())
            return;

        if (_enemy.PlayerInRange())
        {
            // Debug.Log("Atacando");
            Attack();
        }
        else if (_enemy.PlayerInSight())
        {
            _enemy.ChangeState(new ChaseState());
        }
        else if (!_enemy.PlayerInSight())
        {
            _enemy.ChangeState(new IdleState());
        }
    }

    private void Attack()
    {
        _enemy.Attack();
        _enemy.SetAnimation("Attack");
    }

    public void Exit()
    {
        // Debug.Log("Saliendo del estado de ataque");
    }
}