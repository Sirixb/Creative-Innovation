using UnityEngine;

public class RangeAttack : AttackStrategy
{
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float arrowSpeed = 5f;
    

    public override void Attack(Transform attacker, Transform target)
    {
        Vector2 direction = (target.position - firePoint.position).normalized;
        var lance = Instantiate(arrowPrefab, firePoint.position, Quaternion.identity);
        lance.GetComponent<Lance>().Damage = Damage;
        lance.GetComponent<Rigidbody2D>().velocity = direction * arrowSpeed;

        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        lance.transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
    }
}