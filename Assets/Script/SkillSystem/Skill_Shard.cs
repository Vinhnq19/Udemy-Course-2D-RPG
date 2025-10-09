using System.Collections;
using UnityEngine;

public class Skill_Shard : Skill_Base
{
    private Skill_ObjectShard currentShard;
    private Entity_Health playerHealth;
    [SerializeField] private GameObject shardPrefab;
    [SerializeField] private float detonateTime = 2f;

    [Header("Moving Shard Upgrade")]
    [SerializeField] private float shardSpeed = 7f; // speed
    [Header("Multi Cast Upgrade")]
    [SerializeField] private int maxCharges = 3;
    [SerializeField] private int currentCharges;
    [SerializeField] private bool isRecharging;

    [Header("Teleport Regular Upgrade")]
    [SerializeField] private float shardExistDuration = 10f;
    [Header("Teleport Hp Rewind Upgrade")]
    [SerializeField] private float hpRewindPercent = 0.5f;
    public override void TryUseSkill()
    {
        if (!CanUseSkill())
        {
            return;
        }
        if (Unlocked(SkillUpgradeType.Shard))
        {
            HandleShardRegular();
        }
        if (Unlocked(SkillUpgradeType.Shard_MoveToEnemy))
        {
            HandleShardMoving();
        }
        if (Unlocked(SkillUpgradeType.Shard_MultiCast))
        {
            HandleShardMultiCast();
        }
        if (Unlocked(SkillUpgradeType.Shard_Teleport))
        {
            HandleShardTeleport();
        }
        if (Unlocked(SkillUpgradeType.Shard_TeleportHpRewind))
        {
            HandleShardHpRewind();
        }
    }

    protected override void Awake()
    {
        base.Awake();
        playerHealth = player.GetComponent<Entity_Health>();
        currentCharges = maxCharges;
    }

    public void CreateShard()
    {
        float detonateTime = GetDetonateTime();
        GameObject shard = Instantiate(shardPrefab, transform.position, Quaternion.identity);
        currentShard = shard.GetComponent<Skill_ObjectShard>();
        currentShard.SetupShard(this);

        if (Unlocked(SkillUpgradeType.Shard_Teleport) || Unlocked(SkillUpgradeType.Shard_TeleportHpRewind))
        {
            currentShard.OnExplode += ForceCoolDown;
        }
    }
    public void CreateRawShard(Transform target = null, bool shardsCanMove = false)
    {
        bool canMove = shardsCanMove != false ? shardsCanMove :
     Unlocked(SkillUpgradeType.Shard_MoveToEnemy) || Unlocked(SkillUpgradeType.Shard_MultiCast);
        GameObject shard = Instantiate(shardPrefab, transform.position, Quaternion.identity);
        shard.GetComponent<Skill_ObjectShard>().SetupShard(this, detonateTime, canMove, shardSpeed, target);
    }

    public void CreatDomainShard(Transform target)
    {

    }

    private void HandleShardHpRewind()
    {
        if (currentShard == null)
        {
            CreateShard();
            hpRewindPercent = playerHealth.GetHealthPercent();
            return;
        }
        else
        {
            SwapPlayerAndShard();
            playerHealth.SetHealthToPercent(hpRewindPercent);
            SetSkillOnCoolDown();
        }
    }

    private void HandleShardTeleport()
    {
        if (currentShard == null)
            CreateShard();
        else
        {
            SwapPlayerAndShard();
            SetSkillOnCoolDown();
        }

    }

    private void SwapPlayerAndShard()
    {
        Vector3 shardPosition = currentShard.transform.position;
        Vector3 playerPosition = player.transform.position;

        currentShard.transform.position = playerPosition;
        currentShard.Explode();
        player.TeleportPlayer(shardPosition);
    }

    private void HandleShardMultiCast()
    {
        if (currentCharges <= 0)
            return;
        CreateShard();
        currentShard.MoveTowardsClosestTarget(shardSpeed);
        currentCharges--;

        if (isRecharging == false)
        {
            StartCoroutine(ShardRecharge());
        }
    }

    private IEnumerator ShardRecharge()
    {
        isRecharging = true;
        while (currentCharges < maxCharges)
        {
            yield return new WaitForSeconds(coolDown);
            currentCharges++;
        }

        isRecharging = false;
    }

    private void HandleShardMoving()
    {
        CreateShard();
        currentShard.MoveTowardsClosestTarget(shardSpeed);
        SetSkillOnCoolDown();
    }
    private void HandleShardRegular()
    {
        CreateShard();
        SetSkillOnCoolDown();
    }


    public float GetDetonateTime()
    {
        if (Unlocked(SkillUpgradeType.Shard_Teleport) || Unlocked(SkillUpgradeType.Shard_TeleportHpRewind))
        {
            return shardExistDuration;
        }
        return detonateTime;
    }

    private void ForceCoolDown()
    {
        if (OnCoolDown() == false)
        {
            SetSkillOnCoolDown();
            currentShard.OnExplode -= ForceCoolDown;
        }
    }
}
