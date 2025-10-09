using Unity.VisualScripting;
using UnityEngine;

public class Skill_Dash : Skill_Base
{

    public void OnStartEffect()
    {
        if(Unlocked(SkillUpgradeType.Dash_ShardOnStart) || Unlocked(SkillUpgradeType.Dash_ShardOnStartAndArrival))
        {
            CreateShard();
        }
        else if (Unlocked(SkillUpgradeType.Dash_CloneOnStart) || Unlocked(SkillUpgradeType.Dash_CloneOnStartAndArriva1))
        {
            CreateClone();
        }
    }

    public void OnEndEffect()
    {
        if (Unlocked(SkillUpgradeType.Dash_ShardOnStartAndArrival))
        {
            CreateShard();
        }
        if (Unlocked(SkillUpgradeType.Dash_CloneOnStartAndArriva1))
        {
            CreateClone();
        }
    }
    private void CreateShard()
    {
        skillManager.shard.CreateRawShard();
    }

    private void CreateClone()
    {
        skillManager.timeEcho.CreateTimeEcho(); 
    }
}
