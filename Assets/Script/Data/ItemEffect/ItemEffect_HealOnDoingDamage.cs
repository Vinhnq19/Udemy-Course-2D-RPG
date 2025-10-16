using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item Effect/Heal On Doing Damage", fileName = "Item Effect Data - Heal On Doing Damage")]

public class ItemEffect_HealOnDoingDamage : ItemEffectDataSO
{
    [SerializeField] private float percentHealedOnAttack = .2f;

    public override void Subscribe(Player player)
    {
        base.Subscribe(player);
        player.playerCombat.OnDoingPhysicalDamage += HealOnDoingDamage;
    }
    public override void Unsubscribe()
    {
        base.Unsubscribe();
        if(player != null)
        {
            player.playerCombat.OnDoingPhysicalDamage -= HealOnDoingDamage;
        }
    }
    private void HealOnDoingDamage(float damage)
    {
       player.health.IncreaseHealth(damage * percentHealedOnAttack);
    }
}
