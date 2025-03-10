using UnityEngine;

public class ChaseState : IEnemyState
{
    private EnemyState _enemyState;

    public void Enter(EnemyState enemyState)
    {
        // Debug.Log("Entrando en estado de persecución");
        this._enemyState = enemyState;
        _enemyState.SetAnimation("Run");
    }

    public void Update()
    {
        // Debug.Log("persecución");
      
        _enemyState.ChasePlayer();

        if (!_enemyState.PlayerInSight())
        {
            _enemyState.ChangeState(new IdleState());
        }
        else if (_enemyState.PlayerInRange())
        {
            _enemyState.ChangeState(new AttackState());
        }
    }

    public void Exit()
    {
        // Debug.Log("Saliendo del estado de persecución");
    }
}