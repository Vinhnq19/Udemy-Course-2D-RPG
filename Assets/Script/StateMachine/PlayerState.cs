using UnityEngine;

public abstract class PlayerState : EntityState
{
    protected Player player;
    protected PlayerInputSet input;
    protected PlayerSkillManager skillManager;


    public PlayerState(Player player, StateMachine stateMachine, string animBoolName) : base(stateMachine, animBoolName)
    {
        this.player = player;
        anim = player.anim;
        rb = player.rb;
        input = player.input;
        stats = player.stats;
        skillManager = player.skillManager;
    }

    public override void Update()
    {
        base.Update();
        //Make player dash when Left Shift is pressed
        if (input.Player.Dash.WasPressedThisFrame() && CanDash())
        {
            skillManager.dash.SetSkillOnCoolDown();
            stateMachine.ChangeState(player.dashState);
        }
    }

    public override void UpdateAnimationParameters()
    {
        base.UpdateAnimationParameters();
        // Code to execute every frame while in this state
        anim.SetFloat("yVelocity", rb.linearVelocity.y);

    }



    private bool CanDash()
    {
        if(skillManager.dash.CanUseSkill() == false)
            return false; // Cannot dash if skill is on cooldown
        if (player.wallDetected)
            return false; // Cannot dash if touching a wall
        if (stateMachine.currentState == player.dashState)
            return false; // Cannot dash if already dashing
        return true;
    }
}
