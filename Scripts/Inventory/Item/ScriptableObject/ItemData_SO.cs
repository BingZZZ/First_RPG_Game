using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType { SINGLEHAND, DOUBLEHANDS }
public enum ItemType { Usable, Weapon, Armor}
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item Data")]
public class ItemData_SO : ScriptableObject
{
    public ItemType itemType;
    public PlayerStyle playerStyle;
    public string itemName;
    public Sprite itemIcon;
    public int itemNum;
    [TextArea]
    public string itemDescript = "";
    public bool stackable;

    [Header("Weapon")]
    public GameObject weaponPrefab;
    public AttackData_SO weaponData;
    [Header("Usable")]
    public ActionItemData_SO actionItemData;
}
