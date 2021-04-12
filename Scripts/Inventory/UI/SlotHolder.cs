using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public enum SlotType { BAG, WEAPON, ARMOR, ACTION, SHOP}
public class SlotHolder : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public SlotType slotType;
    public ItemUI itemUI;

    private void OnEnable()
    {
        if (slotType == SlotType.SHOP)
        {
            DragItem itemDragUI = itemUI.GetComponent<DragItem>();
            Destroy(itemDragUI);
        }
    }

    private void OnDisable()
    {
        InventoryManager.Instance.itemTipUI.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (slotType != SlotType.SHOP && (eventData.clickCount % 2 == 0 || eventData.button == PointerEventData.InputButton.Right))
        {
            UseItem();
        } else if (slotType == SlotType.SHOP && eventData.button == PointerEventData.InputButton.Right)
        {
            InventoryManager.Instance.inventoryData.AddItem(itemUI.GetItem(), 1);
            InventoryManager.Instance.inventoryUI.RefreshHolders();
        }
    }

    public void UseItem()
    {
        if (itemUI.GetItem() == null) return;
        if (itemUI.GetItem().itemType == ItemType.Usable && itemUI.bag.items[itemUI.itemIndex].amount > 0)
        {
            GameManager.Instance.player.ApplyHealth(itemUI.GetItem().actionItemData.healthPoint);
            itemUI.bag.items[itemUI.itemIndex].amount -= 1;
        }
        UpdataItem();
    }

    public void UpdataItem()
    {
        switch(slotType)
        {
            case SlotType.BAG:
                itemUI.bag = InventoryManager.Instance.inventoryData;
                break;
            case SlotType.WEAPON:
                itemUI.bag = GameManager.Instance.player.characterData.equipmentData;
                if (itemUI.bag.items[itemUI.itemIndex].itemData != null)
                {
                    Instantiate(itemUI.bag.items[itemUI.itemIndex].itemData.weaponPrefab, GameManager.Instance.player.weaponSlot);
                } else
                {
                    Instantiate(GameManager.Instance.player.originalWeaponPrefab, GameManager.Instance.player.weaponSlot);
                }
                break;
            case SlotType.ARMOR:
                itemUI.bag = InventoryManager.Instance.equipmentData;
                break;
            case SlotType.ACTION:
                itemUI.bag = InventoryManager.Instance.actionData;
                break;
            case SlotType.SHOP:
                itemUI.bag = InventoryManager.Instance.shopData;
                break;
            default:
                break;
        }
        var item = itemUI.bag.items[itemUI.itemIndex];
        itemUI.SetUpItemUI(item.itemData, item.amount);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemUI.GetItem())
        {
            InventoryManager.Instance.itemTipUI.SetItemTip(itemUI.GetItem().itemName, itemUI.GetItem().itemDescript);
            InventoryManager.Instance.itemTipUI.gameObject.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        InventoryManager.Instance.itemTipUI.gameObject.SetActive(false);
    }
}
