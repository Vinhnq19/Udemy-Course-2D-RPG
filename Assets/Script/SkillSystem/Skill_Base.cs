using UnityEngine;

public class Skill_Base : MonoBehaviour
{
    public PlayerSkillManager skillManager { get; private set; }
    public Player player { get; private set; }
    public DamageScaleData damageScaleData { get; private set; }
    [Header("General details")]
    [SerializeField] protected SkillType skillType;
    [SerializeField] protected SkillUpgradeType upgradeType;
    [SerializeField] protected float coolDown;
    private float lastUsedTime;

    protected virtual void Awake()
    {
        skillManager = GetComponentInParent<PlayerSkillManager>();
        player = GetComponentInParent<Player>();
        lastUsedTime -= coolDown; 
        damageScaleData = new DamageScaleData();
    }
    public virtual void TryUseSkill()
    {

    }

    public void SetSkillUpgrade(UpgradeData upgrade)
    {
        upgradeType = upgrade.upgradeType;
        coolDown = upgrade.cooldown;
        damageScaleData = upgrade.damageScaleData;
        ResetCoolDown();
    }

    public virtual bool CanUseSkill()
    {
        if(upgradeType == SkillUpgradeType.None)
        {
            Debug.Log($"{this.name} has no upgrade type set.");
            return false;
        }
        if (OnCoolDown())

        {
            Debug.Log($"{this.name} is on cooldown.");
            return false;
        }

        SetSkillOnCoolDown();
        return true;
    }

    protected bool Unlocked(SkillUpgradeType upgradeToCheck) => upgradeType == upgradeToCheck;

    protected bool OnCoolDown() => Time.time < lastUsedTime + coolDown;
    public void SetSkillOnCoolDown() => lastUsedTime = Time.time;
    public void ReduceCoolDownBy(float coolDownReduction) => lastUsedTime = lastUsedTime + coolDownReduction;
    public void ResetCoolDown() => lastUsedTime = Time.time - coolDown;
}
