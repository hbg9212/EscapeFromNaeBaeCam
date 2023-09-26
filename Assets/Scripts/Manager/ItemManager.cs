using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager instance;

    private void Awake()
    {
        instance = this;
    }

    [Header("In Game Count")]
    public int money;
    public int bomb;

    public enum consumableItem { Coin_Bronze, Coin_Silver, Coin_Gold, Bar_Silver, Bar_Gold };
    public enum accessoriesItem { Accessories1, Accessories2 };
    public enum weaponItem { Weapon1, Weapon2 };

    public GameObject[] ConsumableItemList;
    public GameObject GetConsumableItem(int index)
    {
        return Instantiate(ConsumableItemList[index], transform);
    }

    private IItemDrop testDrop = new ConsumableItemDrop();

    private void Start()
    {
        StartCoroutine("DropTest");
    }

    IEnumerator DropTest()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            testDrop.ItemDrop(transform);
        }
    }
}
