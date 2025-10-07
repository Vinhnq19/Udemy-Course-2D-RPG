using UnityEngine;

public class Skill_TimeEcho : Skill_Base
{
    [SerializeField] private GameObject timeEchoPrefab;
    [SerializeField] private float timeEchoDuration;

    public float GetEchoDuration() => timeEchoDuration;

    public override void TryUseSkill()
    {
        if(CanUseSkill() == false) return;
        CreateTimeEcho();
    }

    public void CreateTimeEcho()
    {
        GameObject timeEcho = Instantiate(timeEchoPrefab, transform.position, Quaternion.identity);
        timeEcho.GetComponent<Skill_ObjectTimeEcho>().SetupEcho(this);
    }
}
