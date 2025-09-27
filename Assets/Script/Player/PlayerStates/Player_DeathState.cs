using UnityEngine;

public class Player_DeathState : PlayerState
{
    public Player_DeathState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        input.Disable();
        rb.simulated = false; // Disable physics simulation for the player
    }
}
