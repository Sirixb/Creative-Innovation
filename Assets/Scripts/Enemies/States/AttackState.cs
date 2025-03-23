using UnityEngine;

public class AttackState : IEnemyState
{
    private EnemyState _enemyState;

   public virtual void Enter(EnemyState enemyState)
    {
        this._enemyState = enemyState;
        // Debug.Log("Entrando en estado de ataque");
        _enemyState.IsAttack = true;
        CallAttack();
    }

    public void Update()
    {
        // Debug.Log("Atacando");
        _enemyState.SetPosition(Vector2.zero);

        if (!_enemyState.CanAttack())
            return;

        // if (_enemyState.PlayerInRange())
        //     CallAttack();
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

    public virtual void Exit()
    {
        // Debug.Log("Saliendo del estado de ataque");
        _enemyState.IsAttack = false ;
        _enemyState.AttackSelectorManager.ChooseAttack();
    }
}