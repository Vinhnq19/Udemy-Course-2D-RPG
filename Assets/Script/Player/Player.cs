using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Player : Entity
{
    public static event Action OnPlayerDeath; // Event triggered when the player dies
    public PlayerInputSet input { get; private set; }

    public Player_IdleState idleState { get; private set; }
    public Player_MoveState moveState { get; private set; }

    public Player_JumpState jumpState { get; private set; }
    public Player_FallState fallState { get; private set; }
    public Player_WallSlideState wallSlideState { get; private set; }
    public Player_WallJumpState wallJumpState { get; private set; }
    public Player_DashState dashState { get; private set; }
    public Player_BasicAttackState basicAttackState { get; private set; }
    public Player_JumpAttackState jumpAttackState { get; private set; }
    public Player_DeathState deathState { get; private set; }
    public Player_CounterAttackState counterAttackState { get; private set; }

    [Header("Attack details")]
    public Vector2[] attackVelocity;
    public Vector2 jumpAttackVelocity = new Vector2(5f, -6f);
    public float attackVelocityDuration = .1f;
    public float comboResetTime = 1f; // Time window to continue the combo
    private Coroutine queueAttackCo;

    [Header("Movement Details")]
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public Vector2 wallJumpForce;
    [Tooltip("How many extra jumps allowed while airborne (e.g., 1 = double jump)")]
    public int maxAirJumps { get; private set; } = 1;

    [Range(0, 1)]
    public float inAirMoveMultiplier = 0.7f;
    [Range(0, 1)]
    public float wallSlideMultiplier = 0.7f;
    [Space]
    public float dashDuration = .25f;
    public float dashSpeed = 20f;

    public Vector2 moveInput { get; private set; }

    // Tracks remaining air jumps (resets when player touches ground or wall-jumps)
    private int airJumpsRemaining;

    protected override void Awake()
    {
        base.Awake();
        input = new PlayerInputSet();
        idleState = new Player_IdleState(this, stateMachine, "idle");
        moveState = new Player_MoveState(this, stateMachine, "move");
        jumpState = new Player_JumpState(this, stateMachine, "jumpFall");
        fallState = new Player_FallState(this, stateMachine, "jumpFall");
        wallSlideState = new Player_WallSlideState(this, stateMachine, "wallSlide");
        wallJumpState = new Player_WallJumpState(this, stateMachine, "jumpFall");
        dashState = new Player_DashState(this, stateMachine, "dash");
        basicAttackState = new Player_BasicAttackState(this, stateMachine, "basicAttack");
        jumpAttackState = new Player_JumpAttackState(this, stateMachine, "jumpAttack");
        deathState = new Player_DeathState(this, stateMachine, "dead");
        counterAttackState = new Player_CounterAttackState(this, stateMachine, "counterAttack");
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
        ResetAirJumps();
    }

    protected override IEnumerator SlowdownEntityCo(float duration, float slowMultiplier)
    {
        float originalSpeed = moveSpeed;
        float originalJumpForce = jumpForce;
        float originalAnimSpeed = anim.speed;
        Vector2 originalWallJump = wallJumpForce;
        Vector2 originalJumpAttack = jumpAttackVelocity;
        Vector2[] originalAttackVelocity =  new Vector2[attackVelocity.Length];
        Array.Copy(attackVelocity, originalAttackVelocity, attackVelocity.Length);

        float speedMultiplier = 1 - slowMultiplier;

        moveSpeed = moveSpeed * speedMultiplier;
        jumpForce = jumpForce * speedMultiplier;
        anim.speed = anim.speed * speedMultiplier;
        wallJumpForce = wallJumpForce * speedMultiplier;
        jumpAttackVelocity = jumpAttackVelocity * speedMultiplier;

        for(int i = 0; i < attackVelocity.Length; i++)
        {
            attackVelocity[i] = attackVelocity[i] * speedMultiplier;
        }
        yield return new WaitForSeconds(duration);

        moveSpeed = originalSpeed;
        jumpForce = originalJumpForce;
        wallJumpForce = originalWallJump;
        jumpAttackVelocity = originalJumpAttack;
        anim.speed = originalAnimSpeed;
        
        for (int i = 0; i < attackVelocity.Length; i++)
        {
            attackVelocity[i] = originalAttackVelocity[i];
        }
    }

    public override void EntityDeath()
    {
        base.EntityDeath();

        OnPlayerDeath?.Invoke(); // Trigger the player death event
        stateMachine.ChangeState(deathState);
    }

    private void OnEnable()
    {
        input.Enable();
        // input.Player.Movement.started; //When the move action is started (e.g., when the key is pressed)
        // input.Player.Movement.canceled; //When the move action is canceled (e.g., when the key is released)
        // input.Player.Movement.performed; //When the move action is performed

        input.Player.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>(); //Read the value of the movement input as a Vector2
        input.Player.Movement.canceled += ctx => moveInput = Vector2.zero; //When the movement input is canceled, set moveInput to zero

    }
    private void OnDisable()
    {
        input.Disable();
    }

    public void EnterAttackStateWithDelay()
    {
        if (queueAttackCo != null)
            StopCoroutine(queueAttackCo);
        queueAttackCo = StartCoroutine(EnterAttackStateWithDelayCo()); // Start the coroutine to enter the attack state with a delay
    }
    private IEnumerator EnterAttackStateWithDelayCo()
    {
        yield return new WaitForEndOfFrame(); // Wait until the end of the frame to ensure all input processing is done
        stateMachine.ChangeState(basicAttackState);
    }

    // Resets the available air-jumps back to the configured maximum
    public void ResetAirJumps()
    {
        airJumpsRemaining = maxAirJumps;
    }

    // Attempt to consume an air-jump. Returns true if there was an air-jump available and it was used.
    public bool UseAirJump()
    {
        if (airJumpsRemaining > 0)
        {
            airJumpsRemaining--;
            return true;
        }
        return false;
    }
}
