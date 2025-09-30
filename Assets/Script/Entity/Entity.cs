using System;
using System.Collections;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public event Action OnFlipped;
    public Animator anim { get; private set; }

    public Rigidbody2D rb { get; private set; }
    public Entity_Stats stats { get; private set; }
    protected StateMachine stateMachine;
    public bool isFacingRight = true;
    public int facingDir { get; private set; } = 1; //1 means facing right, -1 means facing left

    [Header("Collision Detection")]
    [SerializeField] protected LayerMask whatIsGround; 
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform primaryWallCheck;
    [SerializeField] private Transform secondaryWallCheck;
    public bool groundDetected { get; private set; }
    public bool wallDetected { get; private set; }

    //Condition variables
    private bool isKnocked;
    private Coroutine knockbackCoroutine;
    private Coroutine slowDownCoroutine;

    protected virtual void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponent<Entity_Stats>();
        stateMachine = new StateMachine();

    }

    protected virtual void Start()
    {

    }
    protected virtual void Update()
    {
        HandleCollisionDetection();
        stateMachine.UpdateActiveState();
    }


    public void CurrentStateAnimationTrigger()
    {
        stateMachine.currentState.AnimationTrigger();
    }

    public virtual void EntityDeath()
    {
        
    }
    public virtual void SlowDownEntity(float duration, float slowMultiplier)
    {
        if(slowDownCoroutine != null)
            StopCoroutine(slowDownCoroutine);
        slowDownCoroutine = StartCoroutine(SlowdownEntityCo(duration, slowMultiplier));
        
    }

    protected virtual IEnumerator SlowdownEntityCo(float duration, float slowMultiplier)
    {
        yield return null;

    }
    public void ReceiveKnockback(Vector2 knockback, float duration)
    {
        if (knockbackCoroutine != null)
            StopCoroutine(knockbackCoroutine);
        knockbackCoroutine = StartCoroutine(KnockbackCo(knockback, duration));
    }
    public IEnumerator KnockbackCo(Vector2 knockback, float duration)
    {
        isKnocked = true;
        Debug.Log("Knockback");
        rb.linearVelocity = knockback;
        yield return new WaitForSeconds(duration);
        yield return null;
        rb.linearVelocity = Vector2.zero;
        isKnocked = false;
    }
    public void SetVelocity(float xVelocity, float yVelocity)
    {
        rb.linearVelocity = new Vector2(xVelocity, yVelocity);
        HandleFlip(xVelocity);
    }
    public void HandleFlip(float xVelocity)
    {
        if (xVelocity > 0 && isFacingRight == false) Flip();
        else if (xVelocity < 0 && isFacingRight) Flip();

    }
    public void Flip()
    {
        transform.Rotate(0f, 180f, 0f);
        isFacingRight = !isFacingRight;
        facingDir *= -1;

        OnFlipped?.Invoke();
    }
    private void HandleCollisionDetection()
    {
        groundDetected = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround); // Check if the ground is detected below the player
        if (secondaryWallCheck != null)
        {
            wallDetected = Physics2D.Raycast(primaryWallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround)
                        && Physics2D.Raycast(secondaryWallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround); // Check if a wall is detected in front of the player
        }
        else
        {
            wallDetected = Physics2D.Raycast(primaryWallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);
        }
    }



    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + new Vector3(0, -groundCheckDistance));
        Gizmos.DrawLine(primaryWallCheck.position, primaryWallCheck.position + new Vector3(wallCheckDistance * facingDir, 0));

        if (secondaryWallCheck != null)
            Gizmos.DrawLine(secondaryWallCheck.position, secondaryWallCheck.position + new Vector3(wallCheckDistance * facingDir, 0));
    }

}
