using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerInputSet input;
    private StateMachine stateMachine;
    public Player_IdleState idleState { get; private set; }
    public Player_MoveState moveState { get; private set; }
    public Vector2 moveInput { get; private set; }

    private void Awake()
    {
        stateMachine = new StateMachine();
        input = new PlayerInputSet();
        idleState = new Player_IdleState(this, stateMachine, "Idle");
        moveState = new Player_MoveState(this, stateMachine, "Move");
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
}
