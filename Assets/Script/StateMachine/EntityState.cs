using UnityEngine;

public abstract class EntityState
{
    protected StateMachine stateMachine;
    protected string animBoolName;

    protected Animator anim;
    protected Rigidbody2D rb;
    protected float stateTimer; // Timer to track duration in the state
    protected bool triggerCalled;

    public EntityState(StateMachine stateMachine, string animBoolName)
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
        UpdateAnimationParameters();
    }

    public virtual void Exit()
    {
        // Code to execute when exiting the state
        anim.SetBool(animBoolName, false);
    }
    public void AnimationTrigger()
    {
        triggerCalled = true;
    }
    public virtual void UpdateAnimationParameters()
    {
        
    }
}
