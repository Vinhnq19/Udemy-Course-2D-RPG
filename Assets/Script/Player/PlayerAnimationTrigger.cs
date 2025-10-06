using UnityEngine;

public class PlayerAnimationTrigger : Entity_AnimationTrigger
{
    private Player player;
    protected override void Awake()
    {
        base.Awake();
        player = GetComponentInParent<Player>();
    }
    private void ThrowSword() => player.skillManager.swordThrow.ThrowSword();
}
