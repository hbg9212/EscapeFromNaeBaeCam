using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Accessories : PickupItem
{
    [SerializeField] private List<CharacterStats> statsModifier;

    protected override void OnPickedUp(GameObject receiver, ItemData itemData)
    {
        Inventory.instance.AddItem(itemData);
        ItemManager.instance.NoticeOfPanel(itemData);
        CharacterStatsHandler statsHandler = receiver.GetComponent<CharacterStatsHandler>();
        foreach (CharacterStats stat in statsModifier)
        {
            statsHandler.AddStatModifier(stat);
        }
    }
}
