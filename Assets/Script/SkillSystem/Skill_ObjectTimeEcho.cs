using UnityEngine;

public class Skill_ObjectTimeEcho : Skill_ObjectBase
{
    [SerializeField] private GameObject onDeathVfx;
    [SerializeField] private LayerMask whatIsGround;
    private Skill_TimeEcho timeEchoManager;

    public void SetupEcho(Skill_TimeEcho echoManager)
    {
        this.timeEchoManager = echoManager;

        Invoke(nameof(HandleDeath), timeEchoManager.GetEchoDuration());
    }
    private void Update()
    {
        anim.SetFloat("yVelocity", rb.linearVelocity.y);
        StopHorizontalMovement();
    }

    public void HandleDeath()
    {
        Instantiate(onDeathVfx, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void StopHorizontalMovement()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.5f, whatIsGround);

        if (hit.collider != null)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
    }
}
