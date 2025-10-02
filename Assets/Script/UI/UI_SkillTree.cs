using UnityEngine;

public class UI_SkillTree : MonoBehaviour
{
    [SerializeField] private int skillPoints;
    [SerializeField] private UI_TreeConnectHandler[] parentNodes;

    public bool EnoughSkillPoints(int cost)
    {
        return skillPoints >= cost;
    }
    public void RemoveSkillPoints(int cost)
    {
        skillPoints -= cost;
    }
    public void AddSkillPoints(int points)
    {
        skillPoints += points;
    }
    private void Start()
    {
        UpdateAllConnections();
    }

    [ContextMenu("Refund All Skills")]
    public void RefundAllSkills()
    {
        UI_TreeNode[] skillNodes = GetComponentsInChildren<UI_TreeNode>();
        foreach (var node in skillNodes)
        {
            node.Refund();
        }

    }
    [ContextMenu("Update All Connections")]
    public void UpdateAllConnections()
    {
        foreach (var node in parentNodes)
        {
            node.UpdateAllConnections();
        }
    }
}
