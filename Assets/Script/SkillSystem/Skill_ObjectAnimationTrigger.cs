using UnityEngine;

public class Skill_ObjectAnimationTrigger : MonoBehaviour
{
    private Skill_ObjectTimeEcho timeEcho;
    private void Awake()
    {
        timeEcho = GetComponentInParent<Skill_ObjectTimeEcho>();
    }

    private void AttackTrigger()
    {
        timeEcho.PerformAttack();
    }

    private void TryTerminate(int currentAttackIndex)
    {
        if (currentAttackIndex == timeEcho.maxAttacks)
        {
            timeEcho.HandleDeath();
        }
    }
}
