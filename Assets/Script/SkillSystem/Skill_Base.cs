using UnityEngine;

public class Skill_Base : MonoBehaviour
{
    [Header("General details")]
    [SerializeField] protected SkillType skillType;
    [SerializeField] protected SkillUpgradeType upgradeType;
    [SerializeField] private float coolDown;
    private float lastUsedTime;

    protected virtual void Awake()
    {
        lastUsedTime -= coolDown; 
    }

    public void SetSkillUpgrade(UpgradeData upgrade)
    {
        upgradeType = upgrade.upgradeType;
        coolDown = upgrade.cooldown;
    }

    public bool CanUseSkill()
    {
        if (OnCoolDown())

        {
            Debug.Log($"{this.name} is on cooldown.");
            return false;
        }

        SetSkillOnCoolDown();
        return true;
    }

    protected bool Unlocked(SkillUpgradeType upgradeToCheck) => upgradeType == upgradeToCheck;

    private bool OnCoolDown() => Time.time < lastUsedTime + coolDown;
    public void SetSkillOnCoolDown() => lastUsedTime = Time.time;
    public void ResetCoolDownBy(float coolDownReduction) => lastUsedTime = lastUsedTime + coolDownReduction;
    public void ResetCoolDown() => lastUsedTime = Time.time;
}
