using UnityEngine;
[CreateAssetMenu(menuName = "RPG Setup/Skill Data", fileName = "Skill data - ")]

public class Skill_Data : ScriptableObject
{
    public int cost;

    [Header("Skill Info")]
    public string displayName;
    [TextArea]
    public string description;
    public Sprite icon;

}
