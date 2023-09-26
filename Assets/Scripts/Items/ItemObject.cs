using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : PickupItem
{
    protected override void OnPickedUp(GameObject receiver, ItemData itemData)
    {
        ItemManager.instance.NoticeOfPanel(itemData);
        foreach (ItemDataConsumable itemDataConsumable in itemData.consumables)
        {
            switch (itemDataConsumable.type)
            {
                case ConsumableType.Gold:
                    ItemManager.instance.gold += (int)itemDataConsumable.value;
                    break;
                case ConsumableType.Bomb:
                    ItemManager.instance.bomb += (int)itemDataConsumable.value;
                    break;
                case ConsumableType.Health:
                    break;
                case ConsumableType.Maxhealth:
                    break;
            }
        }

        if(itemData.type == ItemType.Accessories)
        {
            Inventory.instance.AddItem(itemData);
        }

        if (itemData.type == ItemType.Weapon)
        {

        }
    }
}
