using System.Collections.Generic;
using UnityEngine;

public class Inventory_Storage : Inventory_Base
{
    private Inventory_Player inventory;
    public List<Inventory_Item> materialStash;

    public void AddMaterialToStash(Inventory_Item itemToAdd)
    {
        var stackableItem = StackableItemStash(itemToAdd);
        if (stackableItem != null)
            stackableItem.AddStack();
        else
            materialStash.Add(itemToAdd);

        TriggerUpdateUI();
    }

    public Inventory_Item StackableItemStash(Inventory_Item itemToAdd)
    {
        List<Inventory_Item> stackableItems = materialStash.FindAll(item => item.itemData == itemToAdd.itemData);

        foreach (var stackable in stackableItems)
        {
            if (stackable.CanAddStack())
            {
                return stackable;
            }
        }
        return null;
    }

    public void SetInventory(Inventory_Player inventory)
    {
        this.inventory = inventory;
    }

    public void FromPlayerToStorage(Inventory_Item item, bool transferFullStack)
    {
        int transferAmount = transferFullStack ? item.stackSize : 1;
        for (int i = 0; i < transferAmount; i++)
        {

            if (CanAddItem(item))
            {
                var itemToTransfer = new Inventory_Item(item.itemData);
                inventory.RemoveOneItem(item);
                AddItem(itemToTransfer);
            }
        }
        TriggerUpdateUI();
    }

    public void FromStorageToPlayer(Inventory_Item item, bool transferFullStack)
    {
        int transferAmount = transferFullStack ? item.stackSize : 1;
        for (int i = 0; i < transferAmount; i++)
        {

            if (inventory.CanAddItem(item))
            {
                var itemToTransfer = new Inventory_Item(item.itemData);
                RemoveOneItem(item);
                inventory.AddItem(itemToTransfer);
            }
        }
        TriggerUpdateUI();
    }

}
