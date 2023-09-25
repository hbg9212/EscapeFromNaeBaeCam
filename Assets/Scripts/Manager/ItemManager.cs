using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemManager : MonoBehaviour
{
    public static ItemManager instance;

    private void Awake()
    {
        instance = this;
    }

    [Header("In Game Count")]
    public int money = 0;
    public int bomb = 0;

    [SerializeField] private TextMeshProUGUI _moneyText;
    [SerializeField] private TextMeshProUGUI _bombText;

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

    private void Update()
    {
        _moneyText.text = money.ToString().PadLeft(3, '0');
        _bombText.text = bomb.ToString().PadLeft(3, '0');
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
