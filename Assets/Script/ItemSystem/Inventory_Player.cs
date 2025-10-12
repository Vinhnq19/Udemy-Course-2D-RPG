
using System.Collections.Generic;
using UnityEngine;

public class Inventory_Player : Inventory_Base
{
    private Entity_Stats playerStats;
    public List<Inventory_EquipmentSlot> equipList;
    protected override void Awake()
    {
        base.Awake();
        playerStats = GetComponent<Entity_Stats>();
    }

    public void TryEquipItem(Inventory_Item item)
    {
        var inventory_Item = FindItem(item.itemData);
        var matchingSlots = equipList.FindAll(slot => slot.slotType == item.itemData.itemType);

        foreach (var slot in matchingSlots)
        {
            if (slot.HasItem() == false)
            {
                EquipItem(inventory_Item, slot);
                return;
            }
        }

        // No empty slot found, replace the first one
        var slotToReplace = matchingSlots[0];
        var itemToUnequip = slotToReplace.equippedItem;
        EquipItem(inventory_Item, slotToReplace);
        UnequipItem(itemToUnequip);
    }
    private void EquipItem(Inventory_Item itemToEquip, Inventory_EquipmentSlot slot)
    {
        slot.equippedItem = itemToEquip;
        itemToEquip.AddModifiers(playerStats);

        RemoveItem(itemToEquip);
    }

    public void UnequipItem(Inventory_Item itemToUnequip)
    {
        if (CanAddItem() == false)
        {
            Debug.Log("No space!");
            return;
        }
        foreach (var slot in equipList)
        {
            if (slot.equippedItem == itemToUnequip)
            {
                slot.equippedItem = null;
                break;
            }
        }
        itemToUnequip.RemoveModifiers(playerStats);
        AddItem(itemToUnequip);
    }
}

