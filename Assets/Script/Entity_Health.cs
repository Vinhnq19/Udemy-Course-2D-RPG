using UnityEngine;

public class Entity_Health : MonoBehaviour , IDamagable
{
    private Entity_VFX entityVFX;
    private Entity entity;
    [SerializeField] protected float currentHp;
    [SerializeField] private float maxHealth = 100;
    [SerializeField] protected bool isDead;

    [Header("On Damage Knockback")]
    [SerializeField] private Vector2 knockbackPower = new Vector2(1.5f, 2.5f);
    [SerializeField] private Vector2 heavyKnockbackPower = new Vector2(7f, 7f);
    [SerializeField] private float knockbackDuration = .2f;
    [SerializeField] private float heavyKnockbackDuration = .5f;

    [Header("On Heavy Damage")]
    [SerializeField] private float heaveyDamageThreshold = .3f; //Damage amount that is considered heavy

    protected virtual void Awake()
    {
        entityVFX = GetComponent<Entity_VFX>();
        entity = GetComponent<Entity>();

        currentHp = maxHealth;
    }

    public virtual void TakeDamage(float damage, Transform damageDealer)
    {
        if (isDead) return;

        Vector2 knockback = CalculateKnockback(damage,damageDealer);
        float duration = CalculateDuration(damage);

        entity?.ReceiveKnockback(knockback, duration);
        entityVFX.PlayOnDamageVFX();
        //Knockback
        ReduceHp(damage);
    }

    protected void ReduceHp(float damage)
    {
        currentHp -= (int)damage;
        if (currentHp <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        isDead = true;
        entity.EntityDeath();
    }

    private Vector2 CalculateKnockback(float damage, Transform damageDealer)
    {
        int direction = transform.position.x > damageDealer.position.x ? 1 : -1;
        Vector2 knockback = IsHeavyDamage(damage) ? heavyKnockbackPower : knockbackPower;
        knockback.x *= direction;
        return knockback;
    }

    private float CalculateDuration(float damage) => IsHeavyDamage(damage) ? heavyKnockbackDuration : knockbackDuration;

    private bool IsHeavyDamage(float damage) => damage / maxHealth > heaveyDamageThreshold;
}
