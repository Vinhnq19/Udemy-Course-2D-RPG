using UnityEngine;

public class Player_BasicAttackState : EntityState
{
    private float attackVelocityTimer;


    private const int FirstComboIndex = 1; //We start combo index with number 1, this parameter is used in the Animator
    private int comboIndex = 1;
    private int comboLimit = 3; // Limit to 3 attacks in a combo
    private float lastTimeAttacked;
    public Player_BasicAttackState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
        if(comboLimit != player.attackVelocity.Length)
        {
            Debug.LogWarning("Combo limit does not match the number of attack velocities defined. Adjusting combo limit.");
            comboLimit = player.attackVelocity.Length;
        }
    }
    public override void Enter()
    {
        base.Enter();
        ResetComboIndexIfNeeded();
        anim.SetInteger("basicAttackIndex", comboIndex);

        ApplyAttackVelocity();

    }


    public override void Update()
    {
        base.Update();
        HandleAttackVelocity();
        if (triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        comboIndex++;
        lastTimeAttacked = Time.time;
        
    }
    private void HandleAttackVelocity()
    {
        attackVelocityTimer -= Time.deltaTime;
        if (attackVelocityTimer < 0) // Stop applying attack velocity after duration
            player.SetVelocity(0, rb.linearVelocity.y);
    }
    private void ApplyAttackVelocity()
    {
        Vector2 attackVelocity = player.attackVelocity[comboIndex - 1];
        attackVelocityTimer = player.attackVelocityDuration;
        player.SetVelocity(attackVelocity.x * player.facingDir, attackVelocity.y);
    }
    private void ResetComboIndexIfNeeded()
    {

        if (comboIndex > comboLimit || Time.time - lastTimeAttacked > player.comboResetTime) // Reset combo after 3 attacks
            comboIndex = FirstComboIndex;
    }
}
