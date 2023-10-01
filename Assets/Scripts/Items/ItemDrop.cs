using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItemDrop
{
    void ItemDrop(Transform tf);
}

// 소비형 아이템 중 하나 드랍
public class ConsumableItemDrop : IItemDrop
{
    public void ItemDrop(Transform tf)
    {
        // 드랍 아이템 목록   1코인 5코인 10코인 20코인 50코인 1폭탄 5폭탄
        // 드랍 아이템 가중치 50    20    10     5      1      30    5
        int[] probability = { 50, 20, 10, 5, 1, 30, 5 };

        int target = Probability.Drop(probability);
        Transform item = ItemManager.instance.GetConsumableItem(target).transform;
        item.position = tf.position;
    }
}

// 플레이어가 소지하지 않은 액세서리 아이템 중 하나 드랍
public class AccessoriesItemDrop : IItemDrop
{
    public void ItemDrop(Transform tf)
    {
        List<GameObject> itemList = ItemManager.instance.AccessoriesItemList;
        int[] probability = new int[itemList.Count];
        for(int i = 0; i < itemList.Count; i ++)
        {
            probability[i] = itemList[i].GetComponent<ItemAccessories>().weight;
        }

        int target = Probability.Drop(probability);
        Transform item = ItemManager.instance.GetAccessoriesItem(target).transform;
        item.position = tf.position;

    }
}

// 플레이어가 소지하지 않은 무기 아이템 중 하나 드랍
public class WeaponItemDrop : IItemDrop
{
    public void ItemDrop(Transform tf)
    {
        
    }
}

public static class Probability
{
    public static int Drop(int[] probability)
    {
        int max = 1;
        for(int i = 0; i < probability.Length; i++)
        {
            max += probability[i];
        }
        int ran = Random.Range(0, max);
        int cumulative = 0;
        int target = -1;
        for (int i = 0; i < probability.Length; i++)
        {
            cumulative += probability[i];
            if (ran <= cumulative)
            {
                target = i;
                break;
            }
        }

        return target;
    }
}
