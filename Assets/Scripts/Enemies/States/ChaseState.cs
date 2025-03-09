using UnityEngine;

public class ChaseState : IEnemyState
{
    private Enemy _enemy;

    public void Enter(Enemy enemy)
    {
        // Debug.Log("Entrando en estado de persecución");
        this._enemy = enemy;
        _enemy.SetAnimation("Run");
    }

    public void Update()
    {
        // Debug.Log("persecución");
        _enemy.ChasePlayer();

        if (!_enemy.PlayerInSight())
        {
            _enemy.ChangeState(new IdleState());
        }
        else if (_enemy.PlayerInRange())
        {
            _enemy.ChangeState(new AttackState());
        }
    }

    public void Exit()
    {
        // Debug.Log("Saliendo del estado de persecución");
    }
}