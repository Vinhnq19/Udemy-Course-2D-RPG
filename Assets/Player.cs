using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{

    public Animator anim { get; private set; }

    public Rigidbody2D rb { get; private set; }

    public PlayerInputSet input { get; private set; }
    private StateMachine stateMachine;

    public Player_IdleState idleState { get; private set; }
    public Player_MoveState moveState { get; private set; }

    public Player_JumpState jumpState { get; private set; }
    public Player_FallState fallState { get; private set; }
    public Player_WallSlideState wallSlideState { get; private set; }
    public Player_WallJumpState wallJumpState { get; private set; }
    public Player_DashState dashState { get; private set; }
    public Player_BasicAttackState basicAttackState { get; private set; }
    public Player_JumpAttackState jumpAttackState { get; private set; }
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
    public bool isFacingRight = true;
    public int facingDir { get; private set; } = 1; //1 means facing right, -1 means facing left
    public Vector2 moveInput { get; private set; }

    // Tracks remaining air jumps (resets when player touches ground or wall-jumps)
    private int airJumpsRemaining;

    [Header("Collision Detection")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    public bool groundDetected { get; private set; }
    public bool wallDetected { get; private set; }

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        stateMachine = new StateMachine();
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
    private void Start()
    {
        stateMachine.Initialize(idleState);
        ResetAirJumps();
    }
    private void Update()
    {
        HandleCollisionDetection();
        stateMachine.UpdateActiveState();
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
    public void CallAnimationTrigger()
    {
        stateMachine.currentState.CallAnimationTrigger();
    }
    public void SetVelocity(float xVelocity, float yVelocity)
    {
        rb.linearVelocity = new Vector2(xVelocity, yVelocity);
        HandleFlip(xVelocity);
    }
    private void HandleFlip(float xVelocity)
    {
        if (xVelocity > 0 && isFacingRight == false) Flip();
        else if (xVelocity < 0 && isFacingRight) Flip();

    }
    public void Flip()
    {
        transform.Rotate(0f, 180f, 0f);
        isFacingRight = !isFacingRight;
        facingDir *= -1;
    }
    private void HandleCollisionDetection()
    {
        groundDetected = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround); // Check if the ground is detected below the player
        wallDetected = Physics2D.Raycast(transform.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround); // Check if a wall is detected in front of the player
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, -groundCheckDistance));
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(wallCheckDistance * facingDir, 0));
    }

}
