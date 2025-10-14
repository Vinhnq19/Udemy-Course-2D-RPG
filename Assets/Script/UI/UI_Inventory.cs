using System.Collections.Generic;
using UnityEngine;

public class UI_Inventory : MonoBehaviour
{
    private Inventory_Player inventory;
    private UI_EquipSlot[] uiEquipSlots;

    [SerializeField] private UI_ItemSlotParent inventorySlotParent;
    [SerializeField] private Transform uiEquipSlotParent;

    private void Awake()
    {
        uiEquipSlots = uiEquipSlotParent.GetComponentsInChildren<UI_EquipSlot>();
        inventory = FindFirstObjectByType<Inventory_Player>(); // Assuming there's only one inventory in the scene
        inventory.onInventoryChange += UpdateUI;

        UpdateUI();
    }

    private void UpdateUI()
    {
        inventorySlotParent.UpdateSlots(inventory.itemList);
        UpdateEquipmentSlots();
    }

    private void UpdateEquipmentSlots()
    {
        List<Inventory_EquipmentSlot> playerEquipList = inventory.equipList;
        for(int i = 0; i < uiEquipSlots.Length; i++)
        {
            var playerEquipSlot = playerEquipList[i];

            if (playerEquipSlot.HasItem() == false)
            {
                uiEquipSlots[i].UpdateSlot(null);
            }
            else
            {
                uiEquipSlots[i].UpdateSlot(playerEquipSlot.equippedItem);
            }
        }
    }

    
}
