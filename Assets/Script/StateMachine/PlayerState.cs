using UnityEngine;

public abstract class PlayerState : EntityState
{
    protected Player player;
    protected PlayerInputSet input;


    public PlayerState(Player player, StateMachine stateMachine, string animBoolName) : base(stateMachine, animBoolName)
    {
        this.player = player;
        anim = player.anim;
        rb = player.rb;
        input = player.input;
    }

    public override void Update()
    {
        base.Update();
        // Code to execute every frame while in this state
        anim.SetFloat("yVelocity", rb.linearVelocity.y);
        //Make player dash when Left Shift is pressed
        if (input.Player.Dash.WasPressedThisFrame() && CanDash())
        {
            stateMachine.ChangeState(player.dashState);
        }
    }



    private bool CanDash()
    {
        if (player.wallDetected)
            return false; // Cannot dash if touching a wall
        if (stateMachine.currentState == player.dashState)
            return false; // Cannot dash if already dashing
        return true;
    }
}
