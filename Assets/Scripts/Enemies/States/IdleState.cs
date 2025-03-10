using UnityEngine;

public class IdleState : IEnemyState
{
    private EnemyState _enemyState;
    private float _randomIdleTime;
    private float _counterIdleTime;

    public void Enter(EnemyState enemyState)
    {
        // Debug.Log("Entered IdleState");
        this._enemyState = enemyState;
        _counterIdleTime = 0f;
        _randomIdleTime = Random.Range(_enemyState.MinIdleTime, _enemyState.MaxIdleTime);

        _enemyState.SetAnimation("Idle");
    }

    public void Update()
    {
        // Debug.Log("Idle State");
        
        _enemyState.SetPosition(Vector2.zero);
        _counterIdleTime += Time.deltaTime;

        if (_counterIdleTime >= _randomIdleTime)
            _enemyState.ChangeState(new PatrolState());

        else if (_enemyState.PlayerInSight())
            _enemyState.ChangeState(new ChaseState());
    }

    public void Exit()
    {
        // Debug.Log("Exit IdleState");
    }
}