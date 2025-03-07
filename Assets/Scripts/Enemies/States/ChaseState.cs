using UnityEngine;

public class ChaseState : IEnemyState
{
    private Enemy _enemy;
    public void Enter(Enemy enemy)
    {
        Debug.Log("Entrando en estado de persecución");
        this._enemy = enemy;
        _enemy.SetAnimation(_enemy.RunHash,_enemy.ChaseSpeed);
    }

    public void Update()
    {
        Debug.Log("persecución");
        ChasePlayer();
        
        if (!_enemy.CanSeePlayer()) 
        {
            _enemy.ChangeState(new IdleState()); 
        }
        else if (_enemy.IsInAttackRange()) 
        {
            // _enemy.ChangeState(new AttackState()); 
        }
    }

    private void ChasePlayer()
    {
        var player = _enemy.GetPlayerPosition();
        Vector2 target = player - _enemy.transform.position;
        Debug.DrawRay(_enemy.transform.position, target, Color.red);
        target.Normalize();
        _enemy.SetPosition(target * (_enemy.ChaseSpeed * Time.deltaTime));
        _enemy.SetRotation(target.x);
    }

    public void Exit()
    {
        Debug.Log("Saliendo del estado de persecución");
    }
}