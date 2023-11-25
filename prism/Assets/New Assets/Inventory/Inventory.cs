using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<InventoryItem> inventory = new List<InventoryItem>();
    private Dictionary<ItemData, InventoryItem> itemDictionary = new Dictionary<ItemData, InventoryItem>();

    public void Add(ItemData itemData)
    {
        if (itemDictionary.TryGetValue(itemData, out InventoryItem item))
        {
            item.addWeapon();
        }
    }
}
