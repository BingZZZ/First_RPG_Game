using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    public Image icon = null;
    public Text amount = null;
    public InventoryData_SO bag = null;
    public int itemIndex = -1;

    public void SetUpItemUI(ItemData_SO itemData, int itemAmount)
    {
        if (itemAmount == 0)
        {
            bag.items[itemIndex].itemData = null;
            itemData = null;
        }
        if (itemData != null)
        {
            icon.sprite = itemData.itemIcon;
            if (itemData.itemType == ItemType.Weapon)
            {
                amount.text = "";
            } else
            {
                amount.text = itemAmount.ToString();
            }
            icon.gameObject.SetActive(true);
        } else
        {
            icon.gameObject.SetActive(false);
        }
    }

    public ItemData_SO GetItem()
    {
        return bag.items[itemIndex].itemData;
    }
}
