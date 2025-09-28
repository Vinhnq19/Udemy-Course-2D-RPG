using UnityEngine;

public class Player_CounterAttackState : PlayerState
{
    private PlayerCombat combat;
    private bool counteredSomebody;
    public Player_CounterAttackState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
        combat = player.GetComponent<PlayerCombat>();
    }
    public override void Enter()
    {
        base.Enter();

        stateTimer = combat.GetCounterDuration();
        counteredSomebody = combat.CounterAttackPerformed();
        anim.SetBool("counterAttackPerformed", counteredSomebody);
    }
    public override void Update()
    {
        base.Update();
        player.SetVelocity(0f, rb.linearVelocity.y);

        if (triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }

        // Allow exit even when counter was performed, after animation completes or timer expires
        if (stateTimer < 0 && counteredSomebody == false)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
    public override void Exit()
    {
        base.Exit();
        anim.SetBool("counterAttackPerformed", false);
    }
}
