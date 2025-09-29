
using UnityEngine;

public class Entity_Stats : MonoBehaviour
{
    public Stat maxHealth;
    public Stat_MajorGroup major;
    public Stat_OffensiveGroup offense;
    public Stat_DefendGroup defense;

    public float GetPhysicalDamage(out bool isCrit)
    {
        float baseDamage = offense.damage.GetValue();
        float bonusDamage = major.strength.GetValue() * 2;
        float totalBaseDamage = baseDamage + bonusDamage;

        float baseCritChance = offense.critChance.GetValue();
        float bonusCritChance = major.agility.GetValue() * 0.3f;
        float totalCritChance = baseCritChance + bonusCritChance;

        float baseCritPower = offense.critPower.GetValue();
        float bonusCritPower = major.strength.GetValue() * 0.5f;
        float totalCritPower = (baseCritPower + bonusCritPower) / 100f;

        isCrit = Random.Range(0f, 100f) < totalCritChance;
        float damageResult = isCrit ? totalBaseDamage * totalCritPower : totalBaseDamage;

        return damageResult;
    }

    public float GetMaxHp()
    {
        float baseHp = maxHealth.GetValue();
        float bonusHp = major.vitality.GetValue() * 5;
        return baseHp + bonusHp;
    }
    public float GetEvasion()
    {
        float baseEvasion = defense.evasion.GetValue();
        float bonusEvasion = major.agility.GetValue() * 0.5f;

        float totalEvasion = baseEvasion + bonusEvasion;
        float evasionCap = 85f; //Evasion cannot exceed 85%

        float finalEvasion = Mathf.Clamp(totalEvasion, 0f, evasionCap); //Clamp between 0 and cap
        return finalEvasion;
    }

    public float GetArmorMitigation(float armorReduction)
    {
        float baseArmor = defense.armor.GetValue();
        float bonusArmor = major.vitality.GetValue();
        float totalArmor = baseArmor + bonusArmor;

        float reductionMultiplier = Mathf.Clamp(1 - armorReduction, 0, 1); // Apply armor reduction percentage
        float effectiveArmor = totalArmor * reductionMultiplier;
        float mitigation = effectiveArmor / (effectiveArmor + 100f); //Diminishing return formula
        float mitigationCap = .85f;

        float finalMitigation = Mathf.Clamp(mitigation, 0f, mitigationCap);
        return finalMitigation;
    }

    public float GetArmorReduction()
    {
        //Total armor reduction percentage
        return offense.armorReduction.GetValue() / 100f; //Convert to percentage
    }
    public float GetElementalDamage(out ElementType elementalType)
    {
        float fireDamage = offense.fireDamage.GetValue();
        float iceDamage = offense.iceDamage.GetValue();
        float lightningDamage = offense.lightningDamage.GetValue();

        float bonusElementalDamage = major.intelligence.GetValue();

        float highestDamage = fireDamage;
        elementalType = ElementType.Fire;

        if (iceDamage > highestDamage)
        {
            highestDamage = iceDamage;
            elementalType = ElementType.Ice;
        }
        if (lightningDamage > highestDamage)
        {
            highestDamage = lightningDamage;
            elementalType = ElementType.Lightning;
        }

        if (highestDamage <= 0)
        {
            elementalType = ElementType.None;
            return 0f; // No elemental damage
        }

        float bonusFire = (fireDamage == highestDamage) ? 0 : fireDamage * 0.5f;
        float bonusIce = (iceDamage == highestDamage) ? 0 : iceDamage * 0.5f;
        float bonusLightning = (lightningDamage == highestDamage) ? 0 : lightningDamage * 0.5f;

        float weakerElementsDamage = bonusFire + bonusIce + bonusLightning;
        float finalDamage = highestDamage + bonusElementalDamage + weakerElementsDamage;
        return finalDamage;
    }
    public float GetElementalResistance(ElementType elementType)
    {
        float baseResistance = 0f;
        float bonusResistance = major.intelligence.GetValue() * 0.5f;
        switch (elementType)
        {
            case ElementType.Fire:
                baseResistance = defense.fireRes.GetValue();
                break;
            case ElementType.Ice:
                baseResistance = defense.iceRes.GetValue();
                break;
            case ElementType.Lightning:
                baseResistance = defense.lightningRes.GetValue();
                break;
            case ElementType.None:
                return 0f; // No resistance for non-elemental damage
        }
        float resistance = baseResistance + bonusResistance;
        float resistanceCap = 75f; //Resistance cannot exceed 75%
        float finalResistance = Mathf.Clamp(resistance, 0f, resistanceCap) / 100;
        return finalResistance;
    }
}
