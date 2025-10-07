using System;
using UnityEngine;
using UnityEngine.XR;

public class Enemy_BattleState : EnemyState
{
    private Transform player;
    private Transform lastTarget;
    private float lastTimeWasInBattle;
    public Enemy_BattleState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();

        UpdateBattleTimer();

        if (player == null)
            player = enemy.GetPlayerReference();

        if (ShouldRetreat())
        {
            rb.linearVelocity = new Vector2(enemy.retreatVelocity.x * -DirectionToPlayer(), enemy.retreatVelocity.y);
            enemy.HandleFlip(DirectionToPlayer());
        }
    }

    public override void Update()
    {
        base.Update();

        if (enemy.PlayerDetected())
        {
            UpdateTargetIfNeeded();
            UpdateBattleTimer();
        }

        if (BattleTimeIsOver())
            stateMachine.ChangeState(enemy.idleState);
        if (WithinAttackRange() && enemy.PlayerDetected() == true)
        {
            stateMachine.ChangeState(enemy.attackState);
        }
        else enemy.SetVelocity(enemy.battleMoveSpeed * DirectionToPlayer(), rb.linearVelocity.y);
    }
    private void UpdateTargetIfNeeded()
    {
        if (enemy.PlayerDetected() == false)
            return;
        Transform newTarget = enemy.GetPlayerReference().transform;
        if (newTarget != lastTarget)
        {
            lastTarget = newTarget;
            player = newTarget;
            // Additional logic can be added here if needed when the target changes
        }
    }

    private void UpdateBattleTimer() => lastTimeWasInBattle = Time.time;

    private bool BattleTimeIsOver() => Time.time >= lastTimeWasInBattle + enemy.battleTimeDuration;

    private bool WithinAttackRange() => DistanceToPlayer() < enemy.attackDistance;

    private bool ShouldRetreat() => DistanceToPlayer() < enemy.minRetreatDistance;


    private float DistanceToPlayer()
    {
        if (player == null)
            return float.MaxValue; // If player is not found, return a large distance

        return Mathf.Abs(player.position.x - enemy.transform.position.x); // Only considering horizontal distance
    }

    private int DirectionToPlayer()
    {
        if (player == null)
            return 0; // If player is not found, no direction

        return player.position.x > enemy.transform.position.x ? 1 : -1; // 1 for right, -1 for left
    }
}
