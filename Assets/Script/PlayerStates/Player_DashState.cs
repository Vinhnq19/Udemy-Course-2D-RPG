using UnityEngine;

public class Player_DashState : PlayerState
{
    private float originalGravityScale;
    private float dashDir;
    public Player_DashState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName) { }
    public override void Enter()
    {
        base.Enter();
        dashDir = player.moveInput.x != 0 ? ((int)player.moveInput.x) : player.facingDir;

        stateTimer = player.dashDuration;

        originalGravityScale = rb.gravityScale;
        rb.gravityScale = 0; // Disable gravity during dash
    }
    public override void Update()
    {
        base.Update();
        CancelDashIfNeeded();

        player.SetVelocity(player.dashSpeed * dashDir, 0f);
        // After 1 second, return to idle state
        //State Timer luôn giảm dần theo thời gian, khi chuyển trạng thái dash, stateTimer được đặt lại
        if (stateTimer < 0)
        {
            if (player.groundDetected)
                stateMachine.ChangeState(player.idleState);
            else
                stateMachine.ChangeState(player.fallState);
        }
    }
    public override void Exit()
    {
        base.Exit();
        player.SetVelocity(0f, 0f); // Stop the dash by setting velocity to zero
        rb.gravityScale = originalGravityScale; // Restore original gravity scale
    }
    private void CancelDashIfNeeded()
    {
        if (player.wallDetected)
        {
            if (player.groundDetected)
                stateMachine.ChangeState(player.idleState);
            else
                stateMachine.ChangeState(player.wallSlideState);
        }
    }
}
