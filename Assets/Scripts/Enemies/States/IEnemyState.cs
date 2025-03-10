using UnityEngine;

public interface IEnemyState
{
    void Enter(EnemyState enemyState); 
    void Update(); 
    void Exit(); 
}