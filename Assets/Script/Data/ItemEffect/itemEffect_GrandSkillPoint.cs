using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item Effect/Grand Skill Point", fileName = "Item Effect Data - Grand Skill Point")]

public class ItemEffect_GrandSkillPoint : ItemEffectDataSO
{
    [SerializeField] private int pointToAdd;

    public override void ExecuteEffect()
    {
        UI ui = FindFirstObjectByType<UI>();
        ui.skillTreeUI.AddSkillPoints(pointToAdd);
    }
}
