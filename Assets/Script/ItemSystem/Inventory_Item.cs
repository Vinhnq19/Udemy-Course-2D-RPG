using UnityEngine;
using System;

[Serializable]

public class Inventory_Item
{
    private string itemId;
    public ItemDataSO itemData;
    public int stackSize = 1;

    public ItemModifier[] modifiers { get;  private set; }

    public Inventory_Item(ItemDataSO itemData)
    {
        this.itemData = itemData;

        modifiers = EquipmentData()?.modifiers;
        itemId = itemData.itemName + " - " + Guid.NewGuid();
    }
    public void AddModifiers(Entity_Stats playerStats)
    {
        foreach (var modifier in modifiers)
        {
            Stat statToModify = playerStats.GetStatByType(modifier.statType);
            statToModify.AddModifier(modifier.value, itemId);
        }
    }

    public void RemoveModifiers(Entity_Stats playerStats)
    {
        foreach (var modifier in modifiers)
        {
            Stat statToModify = playerStats.GetStatByType(modifier.statType);
            statToModify.RemoveModifier(itemId);
        }
    }

    private EquipmentDataSO EquipmentData()
    {
        if(itemData is EquipmentDataSO equipmentData)
        {
            return equipmentData;
        }
        return null;
    }

    public bool CanAddStack() => stackSize < itemData.maxStackSize;
    public void AddStack() => stackSize++;
    public void RemoveStack() => stackSize--;
}

