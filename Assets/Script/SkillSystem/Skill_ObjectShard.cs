using UnityEngine;

public class Skill_ObjectShard : Skill_ObjectBase
{
    [SerializeField] private GameObject vfxPrefab;

    public void SetupShard(float detinationTime)
    {
        Invoke(nameof(Explode), detinationTime);
    }

    private void Explode()
    {
        DamageEnemiesInRadius(transform, targetCheckRadius);
        Instantiate(vfxPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() == null)
        {
            return;
        }
        Explode();
    }
}
