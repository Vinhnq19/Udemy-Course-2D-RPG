using UnityEngine;

public class Player_GroundedState : PlayerState
{
    public Player_GroundedState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }
    public override void Update()
    {
        base.Update();

        if (rb.linearVelocity.y < 0 && !player.groundDetected)
        {
            stateMachine.ChangeState(player.fallState);
        }

        // Normal jump from ground
        if (input.Player.Jump.WasPressedThisFrame())
        {
            // When jumping from ground, ensure air-jumps reset so player can double-jump later
            player.ResetAirJumps();
            stateMachine.ChangeState(player.jumpState);
        }
        if (input.Player.Attack.WasPressedThisFrame())
        {
            stateMachine.ChangeState(player.basicAttackState);
        }
        if (input.Player.CounterAttack.WasPressedThisFrame())
        {
            stateMachine.ChangeState(player.counterAttackState);
        }
        if (input.Player.RangeAttack.WasPressedThisFrame() && skillManager.swordThrow.CanUseSkill())
        {
            stateMachine.ChangeState(player.swordThrowState);
        }
    }
}
