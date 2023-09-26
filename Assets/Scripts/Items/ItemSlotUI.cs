using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour
{
    public Button button;
    public Image icon;
    private ItemSlot curSlot;

    public int index;

    public void Set(ItemSlot slot)
    {
        curSlot = slot;
        icon.gameObject.SetActive(true);
        icon.sprite = slot.itemData.icon;
    }

    public void Clear()
    {
        curSlot = null;
        icon.gameObject.SetActive(false);
    }

    public void OnButtonClick()
    {
        Inventory.instance.SelectItem(index);
    }
}
