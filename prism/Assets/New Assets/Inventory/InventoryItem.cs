using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]

public class InventoryItem
{
    public ItemData itemData;
    public bool acquired = false;

    public InventoryItem(ItemData item)
    {
        itemData = item;
        if (!acquired)
        {
            addWeapon();
        }
    }

    public void addWeapon()
    {
        acquired = true;
    }
}
