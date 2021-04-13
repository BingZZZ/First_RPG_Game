using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory/Inventory Data")]
public class InventoryData_SO : ScriptableObject
{
    public List<InventoryItem> items = new List<InventoryItem>();

    public void AddItem(ItemData_SO itemData, int amount)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (itemData == items[i].itemData && itemData.stackable)
            {
                items[i].amount += amount;
                break;
            }
            if (items[i].itemData == null)
            {
                items[i].itemData = itemData;
                items[i].amount = amount;
                break;
            }
        }
    }
}

[System.Serializable]
public class InventoryItem
{
    public ItemData_SO itemData;
    public int amount;
}
