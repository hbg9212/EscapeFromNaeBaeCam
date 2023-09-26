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

        // 50 + 20 + 10 + 5 + 1 + 30 + 5 = 121
        int ran = Random.Range(0, 122);
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
        Transform item = ItemManager.instance.GetConsumableItem(target).transform;
        item.position = tf.position;
    }
}

// �÷��̾ �������� ���� �׼����� ������ �� �ϳ� ���
public class AccessoriesItemDrop : IItemDrop
{
    public void ItemDrop(Transform tf)
    {
      
    }
}

// �÷��̾ �������� ���� ���� ������ �� �ϳ� ���
public class WeaponItemDrop : IItemDrop
{
    public void ItemDrop(Transform tf)
    {
        
    }
}
