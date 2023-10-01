using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAccessories : PickupItem
{
    [SerializeField] private List<CharacterStats> statsModifier;
    public string id;
    public int weight;

    protected override void OnPickedUp(GameObject receiver, ItemData itemData)
    {
        Inventory.instance.AddItem(itemData);
        ItemManager.instance.NoticeOfPanel(itemData);
        CharacterStatsHandler statsHandler = receiver.GetComponent<CharacterStatsHandler>();
        foreach (CharacterStats stat in statsModifier)
        {
            statsHandler.AddStatModifier(stat);
        }

        ItemManager.instance.AccessoriesItemList.RemoveAll(item => item.GetComponent<ItemAccessories>().id == id);
    }
}
