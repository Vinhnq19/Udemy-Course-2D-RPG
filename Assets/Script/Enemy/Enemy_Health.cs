using UnityEngine;

public class Enemy_Health : Entity_Health
{
    private Enemy enemy => GetComponent<Enemy>();
    public override bool TakeDamage(float damage, float elementalDamage, ElementType elementType, Transform damageDealer)
    {
        bool wasHit = base.TakeDamage(damage, elementalDamage, elementType, damageDealer);

        if (!wasHit) return false;

        //if damageDealer is player, enter battle state
        if (damageDealer.GetComponent<Player>() != null)
        {
            enemy.TryEnterBattleState(damageDealer);
        }
        return true;
    }
}
