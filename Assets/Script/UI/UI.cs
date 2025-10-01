using UnityEngine;

public class UI : MonoBehaviour
{
    public UI_SkillToolTip toolTip;

    private void Awake()
    {
     toolTip = GetComponentInChildren<UI_SkillToolTip>();
    }
}
