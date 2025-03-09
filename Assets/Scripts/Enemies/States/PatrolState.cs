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
        // Debug.Log("Entrando en estado de patrulla");
        this._enemy = enemy;
        _counterPatrolTime = 0f;
        _randomPatrolTime = Random.Range(_enemy.MinPatrolTime, _enemy.MaxPatrolTime);
        _randomDirection = Random.insideUnitSphere;
        _randomDirection.Normalize();

        _enemy.SetRotation(_randomDirection.x);
        _enemy.SetAnimation("Run");
    }

    public void Update()
    {
        // Debug.Log("Patrullando");
        _enemy.SetPosition(_randomDirection * (_enemy.PatrolSpeed * Time.deltaTime));

        _counterPatrolTime += Time.deltaTime;
        if (_counterPatrolTime >= _randomPatrolTime)
        {
            _enemy.ChangeState(new IdleState());
        }
        else if (_enemy.PlayerInSight())
        {
            _enemy.ChangeState(new ChaseState());
        }
    }

    public void Exit()
    {
        // Debug.Log("Saliendo del estado de patrulla");
    }
}