using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private SlotHolder currentSlot;
    private SlotHolder targetSlot;

    public void OnBeginDrag(PointerEventData eventData)
    {
        currentSlot = transform.GetComponentInParent<SlotHolder>();
        // Record original data
        InventoryManager.Instance.currentDragData = new InventoryManager.DragData();
        InventoryManager.Instance.currentDragData.originalSlot = transform.GetComponentInParent<SlotHolder>();
        InventoryManager.Instance.currentDragData.originalParent = (RectTransform)transform.parent;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.SetParent(InventoryManager.Instance.dragCanvas.transform, true);
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // if the pointer over UI
        if (EventSystem.current.IsPointerOverGameObject())
        {
            if (InventoryManager.Instance.CheckInInventoryUI(eventData.position) ||
                InventoryManager.Instance.CheckInActionUI(eventData.position) ||
                InventoryManager.Instance.CheckInEquipmentUI(eventData.position))
            {
                if (eventData.pointerEnter.gameObject.GetComponent<SlotHolder>())
                {
                    targetSlot = eventData.pointerEnter.gameObject.GetComponent<SlotHolder>();
                }
                else
                {
                    targetSlot = eventData.pointerEnter.gameObject.GetComponentInParent<SlotHolder>();
                }

                switch (targetSlot.slotType)
                {
                    case SlotType.BAG:
                        if (currentSlot.slotType == SlotType.WEAPON)
                        {
                            GameManager.Instance.player.UnEquipWeapon(currentSlot.itemUI.bag.items[currentSlot.itemUI.itemIndex].itemData);
                        }
                        SwapItem();
                        break;
                    case SlotType.ACTION:
                        if (currentSlot.itemUI.bag.items[currentSlot.itemUI.itemIndex].itemData.itemType == ItemType.Usable)
                            SwapItem();
                        break;
                    case SlotType.WEAPON:
                        if (currentSlot.itemUI.bag.items[currentSlot.itemUI.itemIndex].itemData.itemType == ItemType.Weapon &&
                            currentSlot.itemUI.bag.items[currentSlot.itemUI.itemIndex].itemData.playerStyle == GameManager.Instance.player.characterData.playerStyle)
                        {
                            if (targetSlot.itemUI.bag.items[targetSlot.itemUI.itemIndex].itemData == null)
                            {
                                GameManager.Instance.player.EquipWeapon(currentSlot.itemUI.bag.items[currentSlot.itemUI.itemIndex].itemData);
                            } else
                            {
                                GameManager.Instance.player.ChangeWeapon(targetSlot.itemUI.bag.items[targetSlot.itemUI.itemIndex].itemData, 
                                    currentSlot.itemUI.bag.items[currentSlot.itemUI.itemIndex].itemData);
                            }
                            SwapItem();
                        }
                        break;
                    case SlotType.ARMOR:
                        if (currentSlot.itemUI.bag.items[currentSlot.itemUI.itemIndex].itemData.itemType == ItemType.Armor)
                            SwapItem();
                        break;
                    default:
                        break;
                }

            }
        }
        currentSlot.UpdataItem();
        if (targetSlot != null)
            targetSlot.UpdataItem();
        transform.SetParent(InventoryManager.Instance.currentDragData.originalParent);

        RectTransform tmpT = transform as RectTransform;

        // Drag would let the itemslot to the wrong position, we need to reset it.
        tmpT.offsetMax = -Vector2.one * 5;
        tmpT.offsetMin = Vector2.one * 5;
    }

    public void SwapItem()
    {
        var targetItem = targetSlot.itemUI.bag.items[targetSlot.itemUI.itemIndex];
        var tmpItem = currentSlot.itemUI.bag.items[currentSlot.itemUI.itemIndex];
        bool isSame = targetItem.itemData == tmpItem.itemData;

        if (isSame && targetItem.itemData.stackable)
        {
            tmpItem.amount += targetItem.amount;
            targetItem.itemData = null;
            targetItem.amount = 0;
        }
        currentSlot.itemUI.bag.items[currentSlot.itemUI.itemIndex] = targetItem;
        targetSlot.itemUI.bag.items[targetSlot.itemUI.itemIndex] = tmpItem;
    }
}
