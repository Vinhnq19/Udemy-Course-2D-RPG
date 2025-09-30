using UnityEngine;
using System;
[Serializable]

public class Stat_OffensiveGroup
{
    public Stat attackSpeed;
    //Physical Damage Stats
    public Stat damage;
    public Stat critPower;
    public Stat critChance;
    public Stat armorReduction;

    //Elemental Damage Stats
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightningDamage;
}
