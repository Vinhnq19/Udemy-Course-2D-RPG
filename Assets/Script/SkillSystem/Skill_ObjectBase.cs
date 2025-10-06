using UnityEngine;

public class Skill_ObjectBase : MonoBehaviour
{
    [SerializeField] private GameObject onHitVfx;
    [SerializeField] protected LayerMask whatIsEnemy;
    [SerializeField] protected Transform targetCheck;
    [SerializeField] protected float targetCheckRadius = 1;

    protected Entity_Stats playerStats;
    protected DamageScaleData damageScaleData;
    protected ElementType usedElement;
    protected bool targetGotHit;

    protected void DamageEnemiesInRadius(Transform t, float radius)
    {
        foreach (var target in EnemiesAround(t, radius))
        {
            IDamagable damagable = target.GetComponent<IDamagable>();
            if (damagable == null) continue;

            ElementalEffectData elementalEffectData = new ElementalEffectData(playerStats, damageScaleData);

            float physicalDamage = playerStats.GetPhysicalDamage(out bool isCrit, damageScaleData.physical);
            float elementalDamage = playerStats.GetElementalDamage(out ElementType elementType, damageScaleData.elemental);

            targetGotHit = damagable.TakeDamage(physicalDamage, elementalDamage, elementType, transform);

            if (elementType != ElementType.None)
            {
                target.GetComponent<Entity_StatusHandler>().ApplyStatusEffect(elementType, elementalEffectData);
            }
            if(targetGotHit)
            {
                Instantiate(onHitVfx, target.transform.position, Quaternion.identity);
            }
            usedElement = elementType;
        }
    }

    protected Transform ClostestTarget()
    {
        Transform target = null;
        float closestDistance = Mathf.Infinity;
        foreach (var enemy in EnemiesAround(targetCheck, 10))
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                target = enemy.transform;
                closestDistance = distance;
                Debug.Log($"New closest target found: {target.name}");
            }

        }
        return target;
    }
    protected Collider2D[] EnemiesAround(Transform t, float radius)
    {
        return Physics2D.OverlapCircleAll(t.position, radius, whatIsEnemy);
    }

    protected virtual void OnDrawGizmos()
    {
        if (targetCheck == null)
            targetCheck = transform;

        Gizmos.DrawWireSphere(targetCheck.position, targetCheckRadius);
    }
}
