using System.Collections;
using UnityEngine;

public class Entity_StatusHandler : MonoBehaviour
{
    private Entity entity;
    private Entity_VFX vfx;
    private Entity_Stats stats;

    private Entity_Health health;
    private ElementType currentEffect = ElementType.None;

    [Header("Electricity Effect Details")]
    [SerializeField] private GameObject lightningVfx;
    [SerializeField] private float currentCharge = 0.5f;
    [SerializeField] private float maximumCharge = 1f;
    private Coroutine electricityCo;

    private void Awake()
    {
        entity = GetComponent<Entity>();
        health = GetComponent<Entity_Health>();
        vfx = GetComponent<Entity_VFX>();
        stats = GetComponent<Entity_Stats>();
    }
    public void ApplyStatusEffect(ElementType elementType, ElementalEffectData effectData)
    {
        if(elementType == ElementType.Ice && CanBeApplied(ElementType.Ice))
        {
            ApplyChillEffect(effectData.chillDuration, effectData.chillSlowMultiplier);
        }
        else if(elementType == ElementType.Fire && CanBeApplied(ElementType.Fire))
        {
            ApplyBurnEffect(effectData.burnDuration, effectData.burnDamage);
        }
        else if(elementType == ElementType.Lightning && CanBeApplied(ElementType.Lightning))
        {
            ApplyElectricityEffect(effectData.shockDuration, effectData.shockDamage, effectData.shockCharge);
        }
    }
    public void ApplyElectricityEffect(float duration, float eletricDamage, float charge)
    {
        float lightningResistance = stats.GetElementalResistance(ElementType.Lightning);
        float finalCharge = charge * (1 - lightningResistance);
        currentCharge = currentCharge + finalCharge;
        if (currentCharge >= maximumCharge)
        {
            DoLightningStrike(eletricDamage);
            StopElectricityEffect();
            return;
        }

        if (electricityCo != null)
        {
            StopCoroutine(electricityCo);
        }
        electricityCo = StartCoroutine(ElectricityEffectCo(duration));
    }

    private void StopElectricityEffect()
    {
        currentEffect = ElementType.None;
        currentCharge = 0;
        vfx.StopAllVfx();
    }

    private void DoLightningStrike(float eletricDamage)
    {
        Instantiate(lightningVfx, transform.position, Quaternion.identity);
        health.ReduceHealth(eletricDamage);
    }

    private IEnumerator ElectricityEffectCo(float duration)
    {
        currentEffect = ElementType.Lightning;
        vfx.PlayOnStatusVFX(duration, ElementType.Lightning);
        yield return new WaitForSeconds(duration);
        StopElectricityEffect();
    }

    public void ApplyBurnEffect(float duration, float fireDamage)
    {
        float fireResistance = stats.GetElementalResistance(ElementType.Fire);
        float finalDamage = fireDamage * (1 - fireResistance);
        StartCoroutine(BurnEffectCo(duration, fireDamage));
    }

    private IEnumerator BurnEffectCo(float duration, float totalDamage)
    {
        currentEffect = ElementType.Fire;
        vfx.PlayOnStatusVFX(duration, ElementType.Fire);
        int ticksPersecond = 2;
        int tickCount = Mathf.RoundToInt(duration * ticksPersecond);

        float damagePerTick = totalDamage / tickCount;
        float tickInterval = 1f / ticksPersecond;

        for (int i = 0; i < tickCount; i++)
        {
            health.ReduceHealth(damagePerTick);
            yield return new WaitForSeconds(tickInterval);
        }
        currentEffect = ElementType.None;
    }

    public void ApplyChillEffect(float duration, float slowMultiplier)
    {
        float iceResistance = stats.GetElementalResistance(ElementType.Ice);
        float finalDuration = duration * (1 - iceResistance);
        StartCoroutine(ChilledEffectCo(finalDuration, slowMultiplier));
    }

    private IEnumerator ChilledEffectCo(float duration, float slowMultiplier)
    {
        entity.SlowDownEntity(duration, slowMultiplier);
        currentEffect = ElementType.Ice;
        vfx.PlayOnStatusVFX(duration, ElementType.Ice);
        yield return new WaitForSeconds(duration);
        currentEffect = ElementType.None;
    }

    public bool CanBeApplied(ElementType element)
    {
        if(element == ElementType.Lightning && currentEffect == ElementType.Lightning) return true;
        return currentEffect == ElementType.None;
    }
}
