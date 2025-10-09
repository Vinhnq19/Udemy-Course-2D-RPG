using Unity.Cinemachine;
using UnityEngine;

public class Skill_TimeEcho : Skill_Base
{
    [SerializeField] private GameObject timeEchoPrefab;
    [SerializeField] private float timeEchoDuration;

    [Header("Attack upgrades")]
    [SerializeField] private int maxAttacks = 3;
    [SerializeField] private float duplicateChance = .3f;

    [Header("Heal wisp upgrades")]
    [SerializeField] private float damagePercentHealed = .3f;
    [SerializeField] private float cooldownReducedInSeconds = 3f;

    public float GetPercentOfDamageHealed()
    {
        if(ShouldBeWisp() == false) return 0;
        return damagePercentHealed;
    }

    public float GetCooldownReduceInSeconds()
    {
        if(upgradeType != SkillUpgradeType.TimeEcho_CooldownWisp) return 0;
        return cooldownReducedInSeconds;
    }

    public bool CanRemoveNegativeEffects()
    {
        return upgradeType == SkillUpgradeType.TimeEcho_CleanseWisp;
    }

    public bool ShouldBeWisp()
    {
        return upgradeType == SkillUpgradeType.TimeEcho_CleanseWisp
        || upgradeType == SkillUpgradeType.TimeEcho_HealWisp
        || upgradeType == SkillUpgradeType.TimeEcho_CooldownWisp;
    }

    public float GetDuplicateChance()
    {
        if (upgradeType != SkillUpgradeType.TimeEcho_ChanceToMultiply) return 0;
        return duplicateChance;
    }

    public int GetMaxAttacks()
    {
        if (upgradeType == SkillUpgradeType.TimeEcho_SingleAttack || upgradeType == SkillUpgradeType.TimeEcho_ChanceToMultiply)
            return 1;

        if (upgradeType == SkillUpgradeType.TimeEcho_MultiAttack)
            return maxAttacks;

        return 0;
    }

    public float GetEchoDuration() => timeEchoDuration;

    public override void TryUseSkill()
    {
        if(CanUseSkill() == false) return;
        CreateTimeEcho();
    }

    public void CreateTimeEcho(Vector3? targetPosition = null)
    {
        Vector3 position = targetPosition ?? player.transform.position;
        GameObject timeEcho = Instantiate(timeEchoPrefab, transform.position, Quaternion.identity);
        timeEcho.GetComponent<Skill_ObjectTimeEcho>().SetupEcho(this);
    }
}
