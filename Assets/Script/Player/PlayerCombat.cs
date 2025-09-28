using UnityEngine;

public class PlayerCombat : Entity_Combat
{
    [Header("Counter Attack Details")]
    [SerializeField] private float counterDuration;
    protected override Collider2D[] GetDetectedColliders()
    {
        return base.GetDetectedColliders();
    }

    public bool CounterAttackPerformed()
    {
        bool hasCountered = false;
        foreach (var target in GetDetectedColliders())
        {
            ICounterable counterable = target.GetComponent<ICounterable>();

            if (counterable == null) continue; // Skip if the target is not counterable
            if (counterable.CanBeCountered)
            {
                counterable.HandleCounter();
                hasCountered = true;
            }
        }
        return hasCountered;
    }

    public float GetCounterDuration()
    {
        return counterDuration;
    }
}
