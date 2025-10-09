using System.Collections.Generic;
using UnityEngine;

public class Skill_DomainExpansion : Skill_Base
{
    [SerializeField] private GameObject domainPrefab;

    [Header("Slowing down effect")]
    [SerializeField] private float slowDownPercent = .9f;
    [SerializeField] private float slowDownDomainDuration = 5f;

    [Header("Shard Casting Upgrade")]
    [SerializeField] private int shardToCast = 10;
    [SerializeField] private float shardCastingDomainSlowDown = 1;
    [SerializeField] private float shardCastingDomainDuration = 8f;
    private float spellCastTimer;
    private float spellsPersecond;

    [Header("Time echo cast Upgrade")]
    [SerializeField] private int echoToCast = 8;
    [SerializeField] private float echoCastDomainSlow = 1;
    [SerializeField] private float echoCastDomainDuration = 6f;
    [SerializeField] private float healthToRestoreWithEcho = .5f;

    [Header("Domain details")]
    public float maxDomainSize = 10;
    public float expandSpeed = 3;

    private List<Enemy> trappedTargets = new List<Enemy>();
    private Transform currentTarget;

    public void CreateDomain()
    {
        spellsPersecond = GetSpellsToCast() / GetDomainDuration();
        GameObject domain = Instantiate(domainPrefab, transform.position, Quaternion.identity);
        domain.GetComponent<SkillObject_DomainExpansion>().SetupDomain(this);
    }

    public void DoSpellCasting()
    {
        spellCastTimer -= Time.deltaTime;

        if (currentTarget == null)
        {
            currentTarget = FindTargetInDomain();
        }
        if (currentTarget != null && spellCastTimer <= 0)
        {
            CastSpell(currentTarget);
            spellCastTimer = 1 / spellsPersecond;
            currentTarget = null;
        }
    }

    private void CastSpell(Transform target)
    {
        if (upgradeType == SkillUpgradeType.Domain_EchoSpam)
        {
            Vector3 offset = Random.value < .5f ? new Vector2(1, 0) : new Vector2(-1, 0);
            skillManager.timeEcho.CreateTimeEcho(target.position + offset);
        }
        if (upgradeType == SkillUpgradeType.Domain_ShardSpam)
        {
            skillManager.shard.CreateRawShard(target, true);
        }
    }

    private Transform FindTargetInDomain()
    {
        trappedTargets.RemoveAll(target => target == null || target.health.isDead);

        if (trappedTargets.Count == 0) return null;
        
        int randomIndex = Random.Range(0, trappedTargets.Count);
        return trappedTargets[randomIndex].transform;
    }

    public float GetDomainDuration()
    {
        if (upgradeType == SkillUpgradeType.Domain_SlowingDown) return slowDownDomainDuration;
        else if (upgradeType == SkillUpgradeType.Domain_ShardSpam) return shardCastingDomainDuration;
        else if (upgradeType == SkillUpgradeType.Domain_EchoSpam) return echoCastDomainDuration;
        return 0;
    }
    public float GetSlowDownPercent()
    {
        if (upgradeType == SkillUpgradeType.Domain_SlowingDown) return slowDownPercent;
        else if (upgradeType == SkillUpgradeType.Domain_ShardSpam) return shardCastingDomainSlowDown;
        else if (upgradeType == SkillUpgradeType.Domain_EchoSpam) return echoCastDomainSlow;

        return 0;
    }

    private int GetSpellsToCast()
    {
        if (upgradeType == SkillUpgradeType.Domain_ShardSpam) return shardToCast;
        else if (upgradeType == SkillUpgradeType.Domain_EchoSpam) return echoToCast;
        return 0;
    }
    public bool InstantDomain()
    {
        return upgradeType != SkillUpgradeType.Domain_EchoSpam
        && upgradeType != SkillUpgradeType.Domain_ShardSpam;
    }


    public void AddTarget(Enemy targetToAdd)
    {
        trappedTargets.Add(targetToAdd);
    }
    public void ClearTargets()
    {
        foreach (var enemy in trappedTargets)
        {
            enemy.StopSlowDown();
        }

        trappedTargets = new List<Enemy>();
    }
}
