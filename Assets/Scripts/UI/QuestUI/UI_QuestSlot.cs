using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class UI_QuestSlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI questName;
    [SerializeField] private Image[] rewardQuickReviewSlots;

    public QuestDataSO questInSlot { get; private set; }
    private UI_QuestPreview questPreview;
    public void SetupQuestSlot(QuestDataSO questDataSO)
    {
        questPreview = transform.root.GetComponentInChildren<UI_Quest>().GetQuestPreview();
        questInSlot = questDataSO;
        questName.text = questDataSO.questName;
        foreach (var previwIcon in rewardQuickReviewSlots)
        {
            previwIcon.gameObject.SetActive(false);
        }

        for (int i = 0; i < questInSlot.rewardItems.Length; i++)
        {
            if (questDataSO.rewardItems[i] == null || questDataSO.rewardItems[i].itemData == null) continue;

            Image slot = rewardQuickReviewSlots[i];

            slot.gameObject.SetActive(true);
            slot.sprite = questDataSO.rewardItems[i].itemData.itemIcon;
            slot.GetComponentInChildren<TextMeshProUGUI>().text = questDataSO.rewardItems[i].stackSize.ToString();
        }
    }
    public void UpdateQuestPreview()
    {
        questPreview.SetupQuestPreview(questInSlot);
    }
}
