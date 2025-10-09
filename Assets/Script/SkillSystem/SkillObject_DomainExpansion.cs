using UnityEngine;

public class SkillObject_DomainExpansion : Skill_ObjectBase
{
    private Skill_DomainExpansion domainManager;

    private float expandSpeed = 2f;
    private float duration;
    private float slowDownPercent = .9f;

    private Vector3 targetScale;
    private bool isShrinking;

    public void SetupDomain(Skill_DomainExpansion domainManager)
    {
        this.domainManager = domainManager;

        duration = domainManager.GetDomainDuration();
        float maxSize = domainManager.maxDomainSize;
        slowDownPercent = domainManager.GetSlowDownPercent();
        expandSpeed = domainManager.expandSpeed;

        targetScale = Vector3.one * maxSize;
        Invoke(nameof(ShrinkDomain), duration);
    }

    private void Update()
    {
        HandleScaling();
    }

    private void HandleScaling()
    {
        float sizeDifference = Mathf.Abs(transform.lossyScale.x - targetScale.x); // Assuming uniform scaling
        bool shouldChangeScale = sizeDifference > 0.1f;

        if (shouldChangeScale)
        {
            transform.localScale = Vector3.Lerp(transform.lossyScale, targetScale, Time.deltaTime * expandSpeed);
        }

        if (isShrinking && sizeDifference < 0.1f)
        {
            TerminateDomain();
        }
    }

    private void TerminateDomain()
    {
        domainManager.ClearTargets();
        Destroy(gameObject);
    }

    private void ShrinkDomain()
    {
        targetScale = Vector3.zero;
        isShrinking = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();

        if (enemy == null) return;

        domainManager.AddTarget(enemy);

        enemy.SlowDownEntity(duration, slowDownPercent, true);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();

        if (enemy == null) return;

        enemy.StopSlowDown();

    }
}
