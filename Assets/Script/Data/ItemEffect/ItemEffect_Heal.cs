using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item Effect/Heal Effect", fileName = "Item Effect Data - heal")]

public class ItemEffect_Heal : ItemEffectDataSO
{
    [SerializeField] private float healPercent = .1f;

    public override void ExecuteEffect()
    {
        Player player = FindFirstObjectByType<Player>();
        float healAmount = player.stats.GetMaxHp() * healPercent;
        player.health.IncreaseHealth(healAmount);
    }
}
