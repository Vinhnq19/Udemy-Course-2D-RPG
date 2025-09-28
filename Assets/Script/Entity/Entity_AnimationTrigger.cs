using UnityEngine;

public class Entity_AnimationTrigger : MonoBehaviour
{
    private Entity entity;
    private Entity_Combat entityCombat;
    protected virtual void Awake()
    {
        entity = GetComponentInParent<Entity>();
        entityCombat = GetComponentInParent<Entity_Combat>();
    }
    private void CurrentStateTrigger()
    {
        entity.CurrentStateAnimationTrigger();
    }
    protected void AttackTrigger()
    {
        entityCombat.PerformAttack();
    }
}
