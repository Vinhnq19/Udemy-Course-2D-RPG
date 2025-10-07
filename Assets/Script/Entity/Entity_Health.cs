using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;
public class Entity_Health : MonoBehaviour, IDamagable
{
    private Slider healthBar;
    private Entity_VFX entityVFX;
    private Entity entity;
    private Entity_Stats entityStats;


    [SerializeField] protected float currentHp;
    [SerializeField] protected bool isDead;
    [Header("Health Regeneration")]
    [SerializeField] private float regenInverval = 1;
    [SerializeField] private bool canRenerateHealth = false;

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
        healthBar = GetComponentInChildren<Slider>();
        entityStats = GetComponent<Entity_Stats>();
        if (entityStats != null)
        {
            currentHp = entityStats.GetMaxHp();
            UpdateHealthBar();
            InvokeRepeating(nameof(RegenerateHealth), 0, regenInverval);

        }

    }

    public virtual bool TakeDamage(float damage, float elementalDamage, ElementType elementType, Transform damageDealer)
    {
        if (isDead) return false;

        if (AttackEvaded())
        {
            Debug.Log($"{gameObject.name} Attack Evaded");
            return false;
        }

        Entity_Stats attackerStats = damageDealer.GetComponent<Entity_Stats>();
        float armorReduction = attackerStats != null ? attackerStats.GetArmorReduction() : 0f;

        float mitigation = entityStats != null ? entityStats.GetArmorMitigation(armorReduction) : 0;
        float resistance = entityStats != null ? entityStats.GetElementalResistance(elementType) : 0;
        float physicalDamageTaken = damage * (1 - mitigation);

        float elementalDamageTaken = elementalDamage * (1 - resistance);
        TakeKnockback(damageDealer, physicalDamageTaken);
        //Knockback
        ReduceHealth(physicalDamageTaken + elementalDamageTaken);
        return true;
    }
    private bool AttackEvaded() //Example of evasion check
    {
        if (entityStats == null) return false;
        else
            return Random.Range(0f, 100f) < entityStats.GetEvasion();
    }

    private void TakeKnockback(Transform damageDealer, float finalDamage)
    {
        Vector2 knockback = CalculateKnockback(finalDamage, damageDealer);
        float duration = CalculateDuration(finalDamage);

        entity?.ReceiveKnockback(knockback, duration);
    }


    private void RegenerateHealth()
    {
        if (!canRenerateHealth) return;
        float regenAmount = entityStats.resources.healthRegen.GetValue();
        IncreaseHealth(regenAmount);
    }
    public void IncreaseHealth(float healAmount)
    {
        if (isDead) return;
        float newHealth = currentHp + healAmount;
        float maxHealth = entityStats.GetMaxHp();

        currentHp = Mathf.Min(newHealth, maxHealth);
        UpdateHealthBar();
    }
    protected virtual void Die()
    {
        isDead = true;
        entity?.EntityDeath();
    }

    public float GetHealthPercent() => currentHp / entityStats.GetMaxHp();

    public void SetHealthToPercent(float percent)
    {
        percent = Mathf.Clamp01(percent);
        currentHp = entityStats.GetMaxHp() * percent;
        UpdateHealthBar();
    }
    private void UpdateHealthBar()
    {
        if (healthBar == null)
        {
            return;
        }
        healthBar.value = currentHp / entityStats.GetMaxHp();
    }
    public void ReduceHealth(float damage)
    {
        entityVFX?.PlayOnDamageVFX();
        currentHp -= (int)damage;
        UpdateHealthBar();
        if (currentHp <= 0)
        {
            Die();
        }
    }

    private Vector2 CalculateKnockback(float damage, Transform damageDealer)
    {
        int direction = transform.position.x > damageDealer.position.x ? 1 : -1;
        Vector2 knockback = IsHeavyDamage(damage) ? heavyKnockbackPower : knockbackPower;
        knockback.x *= direction;
        return knockback;
    }

    private float CalculateDuration(float damage) => IsHeavyDamage(damage) ? heavyKnockbackDuration : knockbackDuration;

    private bool IsHeavyDamage(float damage)
    {
        if (entityStats == null) return false;
        else
            return damage / entityStats.GetMaxHp() > heaveyDamageThreshold;
    }
}
