using System;
using UnityEngine;

public class Skill_ObjectShard : Skill_ObjectBase
{
    public event Action OnExplode;
    private Skill_Shard shardManager;
    [SerializeField] private GameObject vfxPrefab;

    private Transform target;
    private float speed;

    private void Update()
    {
        if (target == null)
            return;
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }

    public void MoveTowardsClosestTarget(float moveSpeed, Transform newTarget = null)
    {
        target = newTarget == null ? ClostestTarget() : newTarget;
        this.speed = moveSpeed;
    }

    public void SetupShard(Skill_Shard shardManager)
    {
        this.shardManager = shardManager;

        playerStats = shardManager.player.stats;
        damageScaleData = shardManager.damageScaleData;
        float detonationTime = shardManager.GetDetonateTime();
        Invoke(nameof(Explode), detonationTime);
    }

    public void SetupShard(Skill_Shard shardManager, float detonateTime, bool canMove, float shardSpeed, Transform target = null)
    {
        this.shardManager = shardManager;

        playerStats = shardManager.player.stats;
        damageScaleData = shardManager.damageScaleData;
        Invoke(nameof(Explode), detonateTime);
        
        if(canMove)
        {
            MoveTowardsClosestTarget(shardSpeed, target);
        }

    }

    public void Explode()
    {
        DamageEnemiesInRadius(transform, targetCheckRadius);
        GameObject vfx = Instantiate(vfxPrefab, transform.position, Quaternion.identity);
        vfx.GetComponentInChildren<SpriteRenderer>().color = shardManager.player.playerVFX.GetElementColor(usedElement);

        OnExplode?.Invoke();
        Destroy(gameObject);
    }
 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() == null)
        {
            return;
        }
        Explode();
    }
}
