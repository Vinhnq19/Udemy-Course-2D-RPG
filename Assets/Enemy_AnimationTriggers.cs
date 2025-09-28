using UnityEngine;

public class Enemy_AnimationTriggers : Entity_AnimationTrigger
{
    private Enemy enemy;
    private Enemy_VFX enemyVFX;

    protected override void Awake()
    {
        base.Awake();
        enemy = GetComponentInParent<Enemy>();
        enemyVFX = GetComponentInParent<Enemy_VFX>();


    }
    private void EnableCounterWindow()
    {
        enemyVFX.EnableAttackAlert(true);
        enemy.EnableCounterWindow(true);
    }
    private void DisableCounterWindow()
    {
        enemy.EnableCounterWindow(false);
        enemyVFX.EnableAttackAlert(false);
    }
}
