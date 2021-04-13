using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shop Data", menuName = "NPC/Shop Data")]
public class Shop_SO : ScriptableObject
{
    public List<ItemData_SO> items = new List<ItemData_SO>();
}
