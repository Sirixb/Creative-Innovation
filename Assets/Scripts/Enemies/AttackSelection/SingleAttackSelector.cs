namespace Enemies.AttackSelection
{
    public class SingleAttackSelector : AttackSelectorManager
    {
        public override void ChooseAttack()
        {
            EnemyState.SetAttackStrategy(SelectedAttackStrategy);
        }
    }
}