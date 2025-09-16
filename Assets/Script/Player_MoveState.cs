using UnityEngine;

public class Player_MoveState : Player_GroundedState
{
    public Player_MoveState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }
    public override void Update()
    {
        base.Update();
        if (player.moveInput.x == 0 || player.wallDetected) stateMachine.ChangeState(player.idleState); // Changed to return to idle state if no input or wall detected
        if (player.moveInput.x == 0) stateMachine.ChangeState(player.idleState);

        player.SetVelocity(player.moveInput.x * player.moveSpeed, rb.linearVelocity.y); // Set horizontal velocity based on input, keep vertical velocity unchanged
    }
}
