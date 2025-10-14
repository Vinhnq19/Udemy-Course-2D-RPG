using UnityEngine;

public class Object_BlackSmith : Object_NPC, IInteractable
{
    private Animator anim;
    private Inventory_Player inventory;
    private Inventory_Storage storage;

    protected override void Awake()
    {
        base.Awake();
        storage = GetComponent<Inventory_Storage>();
        anim = GetComponentInChildren<Animator>();
        anim.SetBool("isBlackSmith", true);
    }
    public void Interact()
    {
        ui.storageUI.SetupStorage(inventory, storage);
        ui.storageUI.gameObject.SetActive(true);
    }
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        inventory = player.GetComponent<Inventory_Player>();
        storage.SetInventory(inventory);
    }

    protected override void OnTriggerExit2D(Collider2D other)
    {
        base.OnTriggerExit2D(other);
        ui.SwitchOffAllToolTips();
        ui.storageUI.gameObject.SetActive(false);
    }
}
