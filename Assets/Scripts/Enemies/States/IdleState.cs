using UnityEngine;

public class IdleState : IEnemyState
{
    private Enemy _enemy;
    private float _randomIdleTime;
    private float _counterIdleTime;

    public void Enter(Enemy enemy)
    {
        // Debug.Log("Entered IdleState");
        this._enemy = enemy;
        _counterIdleTime = 0f;
        _randomIdleTime = Random.Range(_enemy.MinIdleTime, _enemy.MaxIdleTime);

        _enemy.SetAnimation("Idle");
    }

    public void Update()
    {
        // Debug.Log("Idle State");
        _enemy.SetPosition(Vector2.zero);

        _counterIdleTime += Time.deltaTime;
        if (_counterIdleTime >= _randomIdleTime)
        {
            _enemy.ChangeState(new PatrolState());
        }
        else if (_enemy.PlayerInSight())
        {
            _enemy.ChangeState(new ChaseState());
        }
    }

    public void Exit()
    {
        // Debug.Log("Exit IdleState");
    }
}