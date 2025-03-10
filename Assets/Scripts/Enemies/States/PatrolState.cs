using System.Security.Cryptography;
using UnityEngine;

public class PatrolState : IEnemyState
{
    private EnemyState _enemyState;
    private Vector2 _randomDirection;
    private float _randomPatrolTime = 0;
    private float _counterPatrolTime = 0;

    public void Enter(EnemyState enemyState)
    {
        // Debug.Log("Entrando en estado de patrulla");
        this._enemyState = enemyState;
        _counterPatrolTime = 0f;
        _randomPatrolTime = Random.Range(_enemyState.MinPatrolTime, _enemyState.MaxPatrolTime);
        _randomDirection = Random.insideUnitSphere;
        _randomDirection.Normalize();

        _enemyState.SetRotation(_randomDirection.x);
        _enemyState.SetAnimation("Run");
    }

    public void Update()
    {
        // Debug.Log("Patrullando");
        
        _enemyState.SetPosition(_randomDirection * (_enemyState.PatrolSpeed * Time.deltaTime));

        _counterPatrolTime += Time.deltaTime;
        if (_counterPatrolTime >= _randomPatrolTime)
        {
            _enemyState.ChangeState(new IdleState());
        }
        else if (_enemyState.PlayerInSight())
        {
            _enemyState.ChangeState(new ChaseState());
        }
    }

    public void Exit()
    {
        // Debug.Log("Saliendo del estado de patrulla");
    }
}