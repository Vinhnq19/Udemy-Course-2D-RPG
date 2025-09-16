using UnityEngine;

public class Player_BasicAttackState : EntityState
{
    private float attackVelocityTimer;
    private float lastTimeAttacked;
    private bool comboAttackQueued;


    private const int FirstComboIndex = 1; //We start combo index with number 1, this parameter is used in the Animator
    private int attackDir;
    private int comboIndex = 1;
    private int comboLimit = 3; // Limit to 3 attacks in a combo
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
        comboAttackQueued = false;
        ResetComboIndexIfNeeded();

        attackDir = player.moveInput.x != 0 ? ((int) player.moveInput.x) : player.facingDir;
        anim.SetInteger("basicAttackIndex", comboIndex);

        ApplyAttackVelocity();

    }


    public override void Update()
    {
        base.Update();
        HandleAttackVelocity();

        if(input.Player.Attack.WasCompletedThisFrame())
        {
            QueueNextAttack();
        }
        if (triggerCalled)
            {
                HandleStateExit();
            }
    }

    public override void Exit()
    {
        base.Exit();
        comboIndex++;
        lastTimeAttacked = Time.time;
        
    }
    private void HandleStateExit()
    {
        if (comboAttackQueued)
                {
                    anim.SetBool(animBoolName, false); // Reset the trigger to avoid immediate re-entry
                    player.EnterAttackStateWithDelay(); // Queue the next attack in the combo
                }
                else
                stateMachine.ChangeState(player.idleState);
    }
    private void QueueNextAttack()
    {
        if (comboIndex < comboLimit)
            comboAttackQueued = true;
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
        player.SetVelocity(attackVelocity.x * attackDir, attackVelocity.y);
    }
    private void ResetComboIndexIfNeeded()
    {

        if (comboIndex > comboLimit || Time.time - lastTimeAttacked > player.comboResetTime) // Reset combo after 3 attacks
            comboIndex = FirstComboIndex;
    }
}
