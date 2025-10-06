using UnityEngine;

public class Entity_Combat : MonoBehaviour
{
    private Entity_VFX vfx;
    private Entity_Stats entityStats;

    public DamageScaleData damageScaleData;
    [Header("Target Detection")]
    [SerializeField] private Transform targetCheck;
    [SerializeField] private float targetCheckRadius = 1f;
    [SerializeField] private LayerMask WhatIsTarget;

    private void Awake()
    {
        vfx = GetComponent<Entity_VFX>();
        entityStats = GetComponent<Entity_Stats>();
    }

    public void PerformAttack()
    {
        foreach (var target in GetDetectedColliders())
        {
            IDamagable damagable = target.GetComponent<IDamagable>();

            if (damagable == null) continue;

            ElementalEffectData elementalEffectData = new ElementalEffectData(entityStats, damageScaleData);

            float elementalDamage = entityStats.GetElementalDamage(out ElementType elementType, 0.6f);
            float damage = entityStats.GetPhysicalDamage(out bool isCrit, 2);
            bool targetGotHit = damagable.TakeDamage(damage, elementalDamage, elementType, transform);

            if(elementType != ElementType.None)
            {
                target.GetComponent<Entity_StatusHandler>().ApplyStatusEffect(elementType, elementalEffectData);
            }

            if (targetGotHit)
            {
                vfx.CreateHitVFX(target.transform, isCrit, elementType);
            }
        }
    }

    // public void ApplyStatusEffect(Transform target, ElementType element, float scaleFactor = 1)
    // {
    //     Entity_StatusHandler statusHandler = target.GetComponent<Entity_StatusHandler>();
    //     if (statusHandler == null) return;
    //     if (element == ElementType.Ice && statusHandler.CanBeApplied(ElementType.Ice))
    //     {
    //         statusHandler.ApplyChillEffect(defaultDuration, chillSlowMultiplier);
    //     }

    //     if (element == ElementType.Fire && statusHandler.CanBeApplied(ElementType.Fire))
    //     {
    //         scaleFactor = fireScale;
    //         float fireDamage = entityStats.offense.fireDamage.GetValue() * scaleFactor;
    //         statusHandler.ApplyBurnEffect(defaultDuration, fireDamage);
    //     }
        
    //     if (element == ElementType.Lightning && statusHandler.CanBeApplied(ElementType.Lightning))
    //     {
    //         scaleFactor = lightningScale;
    //         float lightningDamage = entityStats.offense.lightningDamage.GetValue() * scaleFactor;
    //         statusHandler.ApplyElectricityEffect(defaultDuration, lightningDamage, electrifyChargeBuildUp);
    //     }

    // }

    protected virtual Collider2D[] GetDetectedColliders()
    {
        return Physics2D.OverlapCircleAll(targetCheck.position, targetCheckRadius, WhatIsTarget);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(targetCheck.position, targetCheckRadius);
    }
}
