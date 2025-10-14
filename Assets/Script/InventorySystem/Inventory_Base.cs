using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_Base : MonoBehaviour
{
    public event Action onInventoryChange;
    public int maxInventorySize = 10;
    public List<Inventory_Item> itemList = new List<Inventory_Item>();

    protected virtual void Awake()
    {

    }

    public void TryUseItem(Inventory_Item itemToUse)
    {
        Inventory_Item consumable = itemList.Find(item => item == itemToUse);
        if (consumable == null)
        {
            return;
        }
        consumable.itemEffect.ExecuteEffect();
        if (consumable.stackSize > 1)
        {
            consumable.RemoveStack();
        }
        else
        {
            RemoveOneItem(consumable);
        }

        onInventoryChange?.Invoke();
    }

    public bool CanAddItem(Inventory_Item itemToAdd)
    {
        bool hasStackable = FindStackable(itemToAdd) != null;
        return hasStackable || itemList.Count < maxInventorySize;
    }
    public Inventory_Item FindStackable(Inventory_Item item)
    {
        List<Inventory_Item> stackableItems = itemList.FindAll(i => i.itemData == item.itemData);
        foreach (var stackableItem in stackableItems)
        {
            if (stackableItem.CanAddStack())
                return stackableItem;
        }
        return null;
    }

    public void AddItem(Inventory_Item itemToAdd)
    {

        Inventory_Item itemInInventory = FindStackable(itemToAdd);
        if (itemInInventory != null)
        {
            itemInInventory.AddStack();
        }
        else
        {
            itemList.Add(itemToAdd);
        }
        onInventoryChange?.Invoke();
    }

    public void RemoveOneItem(Inventory_Item itemToRemove)
    {
        Inventory_Item itemInInventory = itemList.Find(item => item == itemToRemove);
        if (itemInInventory.stackSize > 1)
        {
            itemInInventory.RemoveStack();
        }
        else
        itemList.Remove(itemToRemove);
        onInventoryChange?.Invoke();
    }

    public Inventory_Item FindItem(ItemDataSO itemData)
    {
        // Find an item in the inventory that matches the itemData and can still stack
        return itemList.Find(item => item.itemData == itemData);
    }
    public void TriggerUpdateUI()
    {
        onInventoryChange?.Invoke();
    }
}
