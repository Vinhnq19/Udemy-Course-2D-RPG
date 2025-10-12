using UnityEngine;
using System;


[CreateAssetMenu(menuName = "RPG Setup/Skill Data", fileName = "Skill data - ")]

public class Skill_Data : ScriptableObject
{
    public int cost;
    public bool unlockedByDefault;
    public SkillType skillType;
    public UpgradeData upgradeData;

    [Header("Skill Info")]
    public string displayName;
    [TextArea]
    public string description;
    public Sprite icon;

}
[Serializable]
public class UpgradeData
{
    public SkillUpgradeType upgradeType;
    public float cooldown;
    public DamageScaleData damageScaleData;

}

