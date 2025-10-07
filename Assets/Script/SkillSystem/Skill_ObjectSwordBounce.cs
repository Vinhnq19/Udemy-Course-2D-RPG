using UnityEngine;
using System.Collections.Generic;

public class Skill_ObjectSwordBounce : Skill_ObjectSword
{
    [SerializeField] private float bounceSpeed = 15f;
    private int bounceCount;
    private Collider2D[] enemyTargets;
    private Transform nextTarget;
    public List<Transform> selectedBefore = new List<Transform>();

    public override void SetupSword(Skill_SwordThrow swordManager, Vector2 direction)
    {
        anim.SetTrigger("spin");
        base.SetupSword(swordManager, direction);

        bounceSpeed = swordManager.bounceSpeed;
        bounceCount = swordManager.bounceCount;
    }

    protected override void Update()
    {
        HandleComeback();
        HandleBounce();
    }

    private void HandleBounce()
    {
        if (nextTarget == null) return;
        transform.position = Vector2.MoveTowards(transform.position, nextTarget.position, bounceSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, nextTarget.position) < 0.75f)
        {
            DamageEnemiesInRadius(transform, 1);
            BounceToNextTarget();

            if (bounceCount == 0 || nextTarget == null)
            {
                nextTarget = null;
                GetSwordBackToPlayer();
            }
        }
    }

    private void BounceToNextTarget()
    {
        nextTarget = GetNextTarget();
        bounceCount--;
    }

    private Transform GetNextTarget()
    {
        List<Transform> validTarget = GetValidTargets();
        int randomIndex = Random.Range(0, validTarget.Count);
        Transform nextTarget = validTarget[randomIndex];
        selectedBefore.Add(nextTarget);
        return nextTarget;
    }

    private List<Transform> GetValidTargets()
    {
        List<Transform> validTargets = new List<Transform>();
        List<Transform> aliveTargets = GetAliveTargets();
        foreach (var enemy in aliveTargets)
        {
            if (enemy != null && selectedBefore.Contains(enemy) == false)
            {
                validTargets.Add(enemy);
            }
        }
        if (validTargets.Count > 0)
        {
            return validTargets;
        } else
        {
            selectedBefore.Clear();
            return aliveTargets;
        }

    }

    private List<Transform> GetAliveTargets()
    {
        List<Transform> aliveTargets = new List<Transform>();
        foreach (var enemy in enemyTargets)
        {
            if (enemy != null)
                aliveTargets.Add(enemy.transform);
        }
        return aliveTargets;
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (enemyTargets == null)
        {
            enemyTargets = EnemiesAround(transform, 10);
            rb.simulated = false;
        }
        DamageEnemiesInRadius(transform, 1);
        if (enemyTargets.Length <= 1 || bounceCount == 0)
        {
            GetSwordBackToPlayer();
        }
        else {
            nextTarget = GetNextTarget();
        }
    }
}
