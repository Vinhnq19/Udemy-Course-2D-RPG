using UnityEngine;

public class UI_PlayerStats : MonoBehaviour
{
    private UI_StatSlot[] statSlots;
    private Inventory_Player inventory;

    private void Awake()
    {
        statSlots = GetComponentsInChildren<UI_StatSlot>();
        inventory = FindFirstObjectByType<Inventory_Player>();
        inventory.onInventoryChange += UpdateStatsUI;
    }
    private void Start()
    {
        UpdateStatsUI();
    }

    private void UpdateStatsUI()
    {
        foreach (var slot in statSlots)
        {
            slot.UpdateStatValue();
        }
    }
}
