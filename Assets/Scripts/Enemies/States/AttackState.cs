using UnityEngine;

public class AttackState : IEnemyState
{
    private Enemy _enemy;

    public void Enter(Enemy enemy)
    {
        this._enemy = enemy;
        Debug.Log("Entrando en estado de ataque");
        _enemy.SetAnimation(_enemy.AttackHash);
    }

    public void Update()
    {
        Debug.Log("Atacando");
        // _enemy.Attack(); 

        if (!_enemy.IsInAttackRange())
        {
            _enemy.ChangeState(new ChaseState()); 
        }
    }

    public void Exit()
    {
        Debug.Log("Saliendo del estado de ataque");
    }
}