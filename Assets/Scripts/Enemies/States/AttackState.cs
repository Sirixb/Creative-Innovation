using UnityEngine;

public class AttackState : IEnemyState
{
    private EnemyState _enemyState;

    public void Enter(EnemyState enemyState)
    {
        this._enemyState = enemyState;
        // Debug.Log("Entrando en estado de ataque");
        CallAttack();
    }

    public void Update()
    {
        _enemyState.SetPosition(Vector2.zero);

        if (!_enemyState.CanAttack())
            return;

            // Debug.Log("Atacando");
        if (_enemyState.PlayerInRange())
            CallAttack();
        else if (_enemyState.PlayerInSight())
            _enemyState.ChangeState(new ChaseState());
        else if (!_enemyState.PlayerInSight())
            _enemyState.ChangeState(new IdleState());
    }

    private void CallAttack()
    {
        _enemyState.Attack();
        _enemyState.SetAnimation("Attack");
    }

    public void Exit()
    {
        // Debug.Log("Saliendo del estado de ataque");
    }
}