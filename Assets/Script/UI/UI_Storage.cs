using UnityEngine;

public class UI_Storage : MonoBehaviour
{
    private Inventory_Storage storage;
    private Inventory_Player playerInventory;
    [SerializeField] private UI_ItemSlotParent inventorySlotParent;
    [SerializeField] private UI_ItemSlotParent storageSlotParent;
    [SerializeField] private UI_ItemSlotParent materialStashSlotParent;

    public void SetupStorage(Inventory_Player playerInventory, Inventory_Storage storage)
    {
        this.playerInventory = playerInventory;
        this.storage = storage;
        storage.onInventoryChange += UpdateUI;
        UpdateUI();

        UI_StorageSlot[] storageSlots = GetComponentsInChildren<UI_StorageSlot>();

        foreach (var slot in storageSlots)
        {
            slot.SetStorage(storage);
        }
    }

    private void UpdateUI()
    {
        inventorySlotParent.UpdateSlots(playerInventory.itemList);
        storageSlotParent.UpdateSlots(storage.itemList);
        materialStashSlotParent.UpdateSlots(storage.materialStash);
    }
}
