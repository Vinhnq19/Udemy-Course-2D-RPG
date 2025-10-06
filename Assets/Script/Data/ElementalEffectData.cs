using UnityEngine;
using System;

[Serializable]

public class ElementalEffectData
{
    public float chillDuration;
    public float chillSlowMultiplier;

    public float burnDuration;
    public float burnDamage;

    public float shockDuration;
    public float shockDamage;
    public float shockCharge;

    public ElementalEffectData(Entity_Stats entity_Stats, DamageScaleData scaleFactor)
    {
        chillDuration = scaleFactor.chillDuration;
        chillSlowMultiplier = scaleFactor.chillSlowMultiplier;

        burnDuration = scaleFactor.burnDuration;
        burnDamage = entity_Stats.offense.fireDamage.GetValue() * scaleFactor.burnDamageScale;
        shockDuration = scaleFactor.shockDuration;
        shockDamage = entity_Stats.offense.lightningDamage.GetValue() * scaleFactor.shockDamageScale;
        shockCharge = scaleFactor.shockCharge;
    }
}
