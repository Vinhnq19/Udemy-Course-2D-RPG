using System;
using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item Effect/Buff Effect", fileName = "Item Effect Data - buff")]

public class ItemEffect_Buff : ItemEffectDataSO
{
    [SerializeField] private BuffEffectData[] buffsToApply;
    [SerializeField] private float duration = 4f;
    [SerializeField] private string source = Guid.NewGuid().ToString(); // Guid: Global Unique Identifier: unique for each instance

    private Player_Stats playerStats;

    public override bool CanBeUsed()
    {
        if (playerStats == null)
            playerStats = FindAnyObjectByType<Player_Stats>();


        if (playerStats.CanApplyBuffOf(source))
        {
            return true;
        }
        else
        {
            Debug.Log("Same buff effect cannot be applied twice");
            return false;
        }
    }

    public override void ExecuteEffect()
    {
        playerStats.ApplyBuff(buffsToApply, duration, source);
    }

}
