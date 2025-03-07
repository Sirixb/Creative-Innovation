using System.Security.Cryptography;
using UnityEngine;

public class PatrolState : IEnemyState
{
    private Enemy _enemy;
    private Vector2 _randomDirection;
    private float _randomPatrolTime = 0;
    private float _counterPatrolTime = 0;

    public void Enter(Enemy enemy)
    {
        Debug.Log("Entrando en estado de patrulla");
        this._enemy = enemy;
        _counterPatrolTime = 0f;
        _randomPatrolTime = Random.Range(_enemy.MinPatrolTime, _enemy.MaxPatrolTime);
        _randomDirection = Random.insideUnitSphere;
        _randomDirection.Normalize();
        _enemy.SetAnimation(_enemy.RunHash, _enemy.PatrolSpeed);
        _enemy.SetRotation(_randomDirection.x);

    }

    public void Update()
    {
        Debug.Log("Patrullando");
        Patrol();
        _counterPatrolTime += Time.deltaTime;

        if (_counterPatrolTime >= _randomPatrolTime)
        {
            _enemy.ChangeState(new IdleState());
        }
        
        if (_enemy.CanSeePlayer())
        {
            Debug.Log("Vio al player");
            _enemy.ChangeState(new ChaseState());
        }
    }

    private void Patrol()
    {
        var patrolPosition = _randomDirection * (_enemy.PatrolSpeed * Time.deltaTime);
        _enemy.SetPosition(patrolPosition);
    }

    public void Exit()
    {
        Debug.Log("Saliendo del estado de patrulla");
    }
}