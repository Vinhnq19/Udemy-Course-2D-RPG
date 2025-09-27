using UnityEngine;

public class Entity_Combat : MonoBehaviour
{
    public float damage = 10f;
    [Header("Target Detection")]
    [SerializeField] private Transform targetCheck;
    [SerializeField] private float targetCheckRadius = 1f;
    [SerializeField] private LayerMask WhatIsTarget;

    public void  PerformAttack()
    {
        foreach (var target in GetDetectedColliders())
        {
            IDamagable damagable = target.GetComponent<IDamagable>();
            damagable?.TakeDamage(damage, transform);
            Entity_Health targetHealth = target.GetComponent<Entity_Health>();
            
            targetHealth?.TakeDamage(damage, transform);
        }
    }

    private Collider2D[] GetDetectedColliders()
    {
        return Physics2D.OverlapCircleAll(targetCheck.position, targetCheckRadius, WhatIsTarget);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(targetCheck.position, targetCheckRadius);
    }
}
