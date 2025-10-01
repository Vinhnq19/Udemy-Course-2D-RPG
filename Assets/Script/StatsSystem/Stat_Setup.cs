using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Stat Setup", fileName = "Default Stat Setup")]
public class Stat_Setup : ScriptableObject
{
    [Header("Resources")]
    public float maxHealth = 100;
    public float healthRegen;

    [Header("Offense - Physical Damange")]
    public float attackSpeed = 1;
    public float damage = 10;
    public float critChance;
    public float critPower = 150;
    public float armorReduction;

    [Header("Offense = Elemental Damage")]
    public float fireDamage;
    public float iceDamage;
    public float lightningDamage;
    [Header("Defense - Physical Damage")]
    public float armor;
    public float evasion;
    [Header("Defense - Elemental Damage")]
    public float fireResist;
    public float iceResist;
    public float lightningResist;

    [Header("Major Stats")]
    public float strength;
    public float agility;
    public float intelligence;
    public float vitality;
}
