using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public ItemData_SO itemData;
    private bool canPick = false;

    private void Update()
    {
        PickUpItem();
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canPick = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canPick = false;
        }
    }

    private void PickUpItem()
    {
        if (Input.GetKeyDown(KeyCode.E) && canPick)
        {
            if (itemData.itemType == ItemType.Weapon)
            {
                InventoryManager.Instance.inventoryData.AddItem(itemData, itemData.itemNum);
                InventoryManager.Instance.inventoryUI.RefreshHolders();
                GameManager.Instance.player.EquipWeapon(itemData);
                Destroy(this.gameObject);
            }
            if (itemData.itemType == ItemType.Usable)
            {
                InventoryManager.Instance.inventoryData.AddItem(itemData, itemData.itemNum);
                InventoryManager.Instance.inventoryUI.RefreshHolders();
                Destroy(this.gameObject);
            }
        }  
    }
}
