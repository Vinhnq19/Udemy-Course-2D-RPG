using UnityEngine;

public class Entity_AnimationTrigger : MonoBehaviour
{
    private Entity entity;
    private void Awake()
    {
        entity = GetComponentInParent<Entity>();
    }
    private void CurrentStateTrigger()
    {
        entity.CallAnimationTrigger();
    }
}
