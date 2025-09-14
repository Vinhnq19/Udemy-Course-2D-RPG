using UnityEngine;

public abstract class EntityState
{
    protected Player player;
    protected StateMachine stateMachine;
    protected string animBoolName;

    protected Animator anim;
    protected Rigidbody2D rb;
    protected PlayerInputSet input;
    protected float stateTimer; // Timer to track duration in the state
    protected bool triggerCalled;

    public EntityState(Player player, StateMachine stateMachine, string animBoolName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.animBoolName = animBoolName;
        anim = player.anim;
        rb = player.rb;
        input = player.input;
    }
    protected EntityState(StateMachine stateMachine, string animBoolName)
    {
        this.stateMachine = stateMachine;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        // Code to execute when entering the state, every time state will be changed, enter will be called
        anim.SetBool(animBoolName, true);
        triggerCalled = false;
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
        // Code to execute every frame while in this state
        anim.SetFloat("yVelocity", rb.linearVelocity.y);
        //Make player dash when Left Shift is pressed
        if (input.Player.Dash.WasPressedThisFrame() && CanDash())
        {
            stateMachine.ChangeState(player.dashState);
        }
    }

    public virtual void Exit()
    {
        // Code to execute when exiting the state
        anim.SetBool(animBoolName, false);
    }
    public void CallAnimationTrigger()
    {
        triggerCalled = true;
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
