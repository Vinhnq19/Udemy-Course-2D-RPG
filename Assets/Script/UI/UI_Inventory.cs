using System.Collections.Generic;
using UnityEngine;

public class UI_Inventory : MonoBehaviour
{
    private Inventory_Player inventory;
    private UI_ItemSlot[] uiItemSlots;
    private UI_EquipSlot[] uiEquipSlots;

    [SerializeField] private Transform uiItemSlotParent;
    [SerializeField] private Transform uiEquipSlotParent;

    private void Awake()
    {
        uiEquipSlots = uiEquipSlotParent.GetComponentsInChildren<UI_EquipSlot>();
        uiItemSlots = uiItemSlotParent.GetComponentsInChildren<UI_ItemSlot>();
        inventory = FindFirstObjectByType<Inventory_Player>(); // Assuming there's only one inventory in the scene
        inventory.onInventoryChange += UpdateUI;

        UpdateUI();
    }

    private void UpdateUI()
    {
        UpdateInventorySlots();
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

    private void UpdateInventorySlots()
    {
        List<Inventory_Item> itemList = inventory.itemList;
        for(int i = 0; i < uiItemSlots.Length; i++)
        {

            // Update each slot with the corresponding item or clear it if there's no item
            if(i < itemList.Count)
            {
                uiItemSlots[i].UpdateSlot(itemList[i]);
            }
            else
            {
                uiItemSlots[i].UpdateSlot(null);
            }
        }
    }
}
