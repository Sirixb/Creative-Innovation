using UnityEngine;

public class IdleState : IEnemyState
{
    private Enemy _enemy;
    private float _randomIdleTime;
    private float _counterIdleTime;
    private const int IdleSpeed = 0;
    private const float MinRandomTime = 1f;
    private const float MaxRandomTime = 3f;

    public void Enter(Enemy enemy)
    {
        Debug.Log("Entered IdleState");
        this._enemy = enemy;
        _counterIdleTime = 0f;
        _randomIdleTime = Random.Range(MinRandomTime, MaxRandomTime);
        _enemy.SetAnimation(_enemy.RunHash, IdleSpeed);
    }

    public void Update()
    {
        Debug.Log("Idle State");
        _counterIdleTime += Time.deltaTime;

        if (_counterIdleTime >= _randomIdleTime)
        {
            _enemy.ChangeState(new PatrolState());
        }
        
        if (_enemy.CanSeePlayer())
        {
            Debug.Log("Vio al player");
            _enemy.ChangeState(new ChaseState());
        }
    }

    public void Exit()
    {
        Debug.Log("Exit IdleState");
        
    }
}
