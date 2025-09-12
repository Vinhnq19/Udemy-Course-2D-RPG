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


    [Header("Movement Details")]
    public float moveSpeed = 5f;
    public float jumpForce = 5f;

    public bool isFacingRight = true;
    public Vector2 moveInput { get; private set; }

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
    }
    private void Update()
    {
        stateMachine.UpdateActiveState();
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
    private void Flip()
    {
        transform.Rotate(0f, 180f, 0f);
        isFacingRight = !isFacingRight;
    }
}
