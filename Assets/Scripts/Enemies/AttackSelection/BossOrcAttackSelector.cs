using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemies.AttackSelection
{
    public class BossOrcAttackSelector : AttackSelectorManager
    {
        [SerializeField] private AttackStrategy meleeAttack;
        [SerializeField] private AttackStrategy rangeAttack;
        [SerializeField] private SpawnAttack spawnAttack;


        private bool _switchMeleeAndRange = false;

        public override void ChooseAttack()
        {
            var randomValue = Random.value;
            if (randomValue >= 0.9f && spawnAttack.LivingEnemies() < spawnAttack.MaxEnemiesSpawned - 1)
            {
                SelectedAttackStrategy = spawnAttack;
            }
            else
            {
                _switchMeleeAndRange = !_switchMeleeAndRange;
                SelectedAttackStrategy = _switchMeleeAndRange ? rangeAttack : meleeAttack;
            }

            // Debug.Log($"Orc Boss eligió: {selectedAttackStrategy.GetType().Name}");
            EnemyState.SetAttackStrategy(SelectedAttackStrategy);
        }
    
   
    }
}