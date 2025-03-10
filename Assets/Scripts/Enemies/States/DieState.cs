using UnityEngine;

public class DieState: IEnemyState
{
    private EnemyState _enemyState;

    public void Enter(EnemyState enemyState)
    {
        this._enemyState = enemyState;
        // Debug.Log("Entrando en estado de muerte");
        _enemyState.SetAnimation("Die");
    }

    public void Update()
    {
       // Debug.Log("Murio");
       _enemyState.SetPosition(Vector2.zero);

    }

    public void Exit()
    {
        // Debug.Log("saliendo en estado de muerte");
    }
}