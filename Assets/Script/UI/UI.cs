using UnityEngine;

public class UI : MonoBehaviour
{
    public UI_SkillToolTip skillToolTip;
    public UI_ItemTooltip itemToolTip;
    public UI_StatToolTip statToolTip;
    public UI_SkillTree skillTree;
    private bool skillTreeEnabled;

    private void Awake()
    {
        itemToolTip = GetComponentInChildren<UI_ItemTooltip>();
        skillToolTip = GetComponentInChildren<UI_SkillToolTip>();
        statToolTip = GetComponentInChildren<UI_StatToolTip>();
        skillTree = GetComponentInChildren<UI_SkillTree>(true);
    }
    public void ToggleSkillTreeUI()
    {
        skillTreeEnabled = !skillTreeEnabled;
        skillTree.gameObject.SetActive(skillTreeEnabled);
        skillToolTip.ShowToolTip(false, null);
    }
}
