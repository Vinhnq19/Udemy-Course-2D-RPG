using UnityEngine;

public class Player_JumpState : EntityState
{
    public Player_JumpState(Player player, StateMachine stateMachine, string stateName) : base(player, stateMachine, stateName)
    {
    }
    public override void Enter()
    {
        base.Enter();

        //make object go up, increase y velocity
        player.SetVelocity(rb.linearVelocity.x, player.jumpForce);
    }
    public override void Update()
    {
        base.Update();

        if(rb.linearVelocity.y < 0)
        {
            stateMachine.ChangeState(player.fallState);
        }
        //if y velocity < 0, change to fall state
    }
}
