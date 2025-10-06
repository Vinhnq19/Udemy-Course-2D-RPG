using UnityEngine;

public enum SkillUpgradeType
{
    None,
    // Dash Tree
    Dash,
    Dash_CloneOnStart, // create a clone when dash starts
    Dash_CloneOnStartAndArriva1, // create a clone when dash starts and ends
    Dash_ShardOnStart, // create a shard when dash starts
    Dash_ShardOnStartAndArrival, // create a shard when dash starts and ends

    // Shard Tree
    Shard,
    Shard_MoveToEnemy,
    Shard_MultiCast,
    Shard_Teleport,
    Shard_TeleportHpRewind,

    // Sword Throw Tree
    SwordThrow,
    SwordThrow_Spin,
    SwordThrow_Pierce,
    SwordThrow_Bounce


}
