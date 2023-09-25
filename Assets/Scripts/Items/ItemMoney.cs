using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMoney : PickupItem
{
    [SerializeField] private int countValue;

    protected override void OnPickedUp(GameObject receiver)
    {
;       ItemManager.instance.money += countValue;
    }
}
