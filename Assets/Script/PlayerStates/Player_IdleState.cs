using UnityEngine;

public class Player_IdleState : Player_GroundedState
{
    public Player_IdleState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();

        player.SetVelocity(0, rb.linearVelocity.y); //Set horizontal velocity to 0
        // On landing, reset available air-jumps
        player.ResetAirJumps();
    }
    public override void Update()
    {
        base.Update();
        if(player.moveInput.x == player.facingDir && player.wallDetected)
        return; // Prevents moving into wall

        if (player.moveInput.x != 0) stateMachine.ChangeState(player.moveState);
    }
}
