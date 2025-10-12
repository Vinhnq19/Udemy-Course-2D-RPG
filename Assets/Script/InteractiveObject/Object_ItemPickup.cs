using UnityEngine;

public class Object_ItemPickup : MonoBehaviour
{
    private SpriteRenderer sr;
    [SerializeField] private ItemDataSO itemData;

    private Inventory_Item itemToAdd;
    private Inventory_Base playerInventory;

    private void Awake()
    {
        itemToAdd = new Inventory_Item(itemData);

    }

    private void OnValidate()
    {
        if (itemData == null) return; // if no item data, do nothing
        if (sr == null) sr = GetComponent<SpriteRenderer>();
        sr.sprite = itemData.itemIcon;
        gameObject.name = "Object_ItemPickup - " + itemData.itemName;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerInventory = collision.GetComponent<Inventory_Base>();
        if(playerInventory == null) return;

        bool canAddItem = playerInventory.CanAddItem() || playerInventory.FindStackable(itemToAdd) != null;
        if (canAddItem)
        {
            playerInventory.AddItem(itemToAdd);
            Destroy(gameObject);
        }
    }

}
