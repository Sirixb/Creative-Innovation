using UnityEngine;

public class Destructible : MonoBehaviour
{
    [SerializeField] private GameObject destroyedPrefab;
    [SerializeField] private GameObject particles;
    [SerializeField] private float destroyDelay = 0.2f;
    [SerializeField] private DropConsumable dropConsumable;

    private void DestroyObject()
    {
        if (destroyedPrefab == null || particles ==null) return;
        Destroy(gameObject, destroyDelay);
        Instantiate(particles, transform.position, Quaternion.identity);
        dropConsumable.DropItems();
        Instantiate(destroyedPrefab, transform.position, Quaternion.identity);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Damager>() != null)
        {
            DestroyObject();
        }
    }
}