using UnityEngine;

public class Skill_ObjectTimeEcho : Skill_ObjectBase
{
    [SerializeField] private float wispMoveSpeed = 15;
    [SerializeField] private GameObject onDeathVfx;
    [SerializeField] private LayerMask whatIsGround;
    private bool shouldMoveToPlayer = false;

    private Transform playerTransform;
    private Skill_TimeEcho timeEchoManager;
    private TrailRenderer wispTrail;
    private Entity_Health playerHealth;
    private Skill_ObjectHealth objectHealth;
    private PlayerSkillManager skillManager;
    private Entity_StatusHandler statusHandler;

    public int maxAttacks { get; private set; }

    public void SetupEcho(Skill_TimeEcho echoManager)
    {
        this.timeEchoManager = echoManager;
        playerStats = echoManager.player.stats;
        damageScaleData = echoManager.damageScaleData;
        maxAttacks = echoManager.GetMaxAttacks();
        playerTransform = echoManager.transform.root;
        playerHealth = echoManager.player.health;
        skillManager = echoManager.player.skillManager;
        statusHandler = echoManager.player.statusHandler;

        Invoke(nameof(HandleDeath), timeEchoManager.GetEchoDuration());
        FlipToTarget();

        objectHealth = GetComponent<Skill_ObjectHealth>();
        wispTrail = GetComponentInChildren<TrailRenderer>();
        wispTrail.gameObject.SetActive(false);

        anim.SetBool("canAttack", maxAttacks > 0);

    }
    private void Update()
    {
        if (shouldMoveToPlayer)
        {
            HandleWispMovement();
        }
        else
        {
            anim.SetFloat("yVelocity", rb.linearVelocity.y);
            StopHorizontalMovement();
        }
        anim.SetFloat("yVelocity", rb.linearVelocity.y);
        StopHorizontalMovement();
    }

    private void HandleWispMovement()
    {
        transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, wispMoveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, playerTransform.position) < .5f)
        {
            Destroy(gameObject);
            HandlePlayerTouch();
        }
    }

    private void HandlePlayerTouch()
    {
        float healAmount = objectHealth.lastDamageTaken * timeEchoManager.GetPercentOfDamageHealed();
        playerHealth.IncreaseHealth(healAmount);

        float amountInSecond = timeEchoManager.GetCooldownReduceInSeconds();
        skillManager.ReduceAllSkillCooldownBy(amountInSecond);

        if(timeEchoManager.CanRemoveNegativeEffects())
        {
            statusHandler.RemoveAllNegativeEffects();
        }
    }

    private void FlipToTarget()
    {
        Transform target = ClostestTarget();

        if (target != null && target.position.x < transform.position.x)
            transform.Rotate(0, 180, 0);
    }
    public void PerformAttack()
    {
        DamageEnemiesInRadius(targetCheck, 1);
        if (targetGotHit == false) return;

        bool canDuplicate = Random.value < timeEchoManager.GetDuplicateChance();
        float xOffset = transform.position.x < lastTarget.position.x ? 1 : -1;

        if (canDuplicate)
            timeEchoManager.CreateTimeEcho(lastTarget.position + new Vector3(xOffset, 0));
    }

    public void HandleDeath()
    {
        Instantiate(onDeathVfx, transform.position, Quaternion.identity);
        if (timeEchoManager.ShouldBeWisp())
        {
            TurnIntoWisp();

        }
        else

            Destroy(gameObject);
    }

    private void TurnIntoWisp()
    {
        shouldMoveToPlayer = true;
        anim.gameObject.SetActive(false);
        wispTrail.gameObject.SetActive(true);
        rb.simulated = false;
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
