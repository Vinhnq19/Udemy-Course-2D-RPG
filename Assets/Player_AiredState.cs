using UnityEngine;

public class Player_AiredState : EntityState
{
    public Player_AiredState(Player player, StateMachine stateMachine, string stateName) : base(player, stateMachine, stateName)
    {
    }

    public override void Update()
    {
        base.Update();
        if (player.moveInput.x != 0)
        {
            player.SetVelocity(player.moveInput.x * (player.moveSpeed * player.inAirMoveMultiplier), rb.linearVelocity.y); //Maintain horizontal velocity based on input
        }

        // Allow an additional air-jump (double-jump) if player has any remaining
        if (input.Player.Jump.WasPressedThisFrame())
        {
            // Try to consume an air jump. If successful, apply jump force.
            if (player.UseAirJump())
            {
                player.SetVelocity(rb.linearVelocity.x, player.jumpForce);
                // optionally change state to jumpState to reuse Enter logic/animation
                stateMachine.ChangeState(player.jumpState);
            }
        }
        if(input.Player.Attack.WasPressedThisFrame())
        {
            stateMachine.ChangeState(player.jumpAttackState);
        }
    }
}

