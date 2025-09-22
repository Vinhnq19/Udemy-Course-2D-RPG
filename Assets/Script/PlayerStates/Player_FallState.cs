using UnityEngine;

public class Player_FallState : Player_AiredState
{
    public Player_FallState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();

        //If player detects ground, change to idle or move state based on input
        if (player.groundDetected)
        {
            stateMachine.ChangeState(player.idleState);
        }
        if(player.wallDetected)
        {
            stateMachine.ChangeState(player.wallSlideState);
        }
    }
}
