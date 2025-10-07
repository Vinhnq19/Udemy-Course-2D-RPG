using UnityEngine;

public class Skill_ObjectHealth : Entity_Health
{
    protected override void Die()
    {
        Skill_ObjectTimeEcho timeEcho = GetComponent<Skill_ObjectTimeEcho>();
        timeEcho.HandleDeath();
    }
}
