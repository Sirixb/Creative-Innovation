using UnityEngine;

namespace Enemies.AttackSelection
{
    public abstract class AttackSelectorManager : MonoBehaviour
    {
        protected EnemyState EnemyState;
        protected AttackStrategy SelectedAttackStrategy;

        protected void Awake()
        {
            SetDefaultAttack();
        }

        private void SetDefaultAttack()
        {
            EnemyState = GetComponent<EnemyState>();
            SelectedAttackStrategy = GetComponentInChildren<AttackStrategy>();
            EnemyState.SetAttackStrategy(SelectedAttackStrategy);
        }

        public abstract void ChooseAttack();
    }
}