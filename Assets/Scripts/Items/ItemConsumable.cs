using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemConsumable : PickupItem
{
    protected override void OnPickedUp(GameObject receiver, ItemData itemData)
    {
        ItemManager.instance.NoticeOfPanel(itemData);
        foreach (ItemDataAddition addition in itemData.addition)
        {
            switch (addition.type)
            {
                case AdditionType.Gold:
                    ItemManager.instance.gold += (int)addition.value;
                    break;
                case AdditionType.Bomb:
                    ItemManager.instance.bomb += (int)addition.value;
                    break;
                case AdditionType.Health:
                    break;
            }
        }
    }
}
