using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItemDrop
{
    void ItemDrop(Transform tf);
}

// �Һ��� ������ �� �ϳ� ���
public class ConsumableItemDrop : IItemDrop
{
    public void ItemDrop(Transform tf)
    {
        // ��� ������ ���   1���� 5���� 10���� 20���� 50���� 1��ź 5��ź
        // ��� ������ ����ġ 50    20    10     5      1      30    5
        int[] probability = { 50, 20, 10, 5, 1, 30, 5 };

        int target = Probability.Drop(probability);
        Transform item = ItemManager.instance.GetConsumableItem(target).transform;
        item.position = tf.position;
    }
}

// �÷��̾ �������� ���� �׼����� ������ �� �ϳ� ���
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

// �÷��̾ �������� ���� ���� ������ �� �ϳ� ���
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
