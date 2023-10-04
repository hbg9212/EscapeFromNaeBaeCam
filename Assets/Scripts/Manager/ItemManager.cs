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
    public int gold;
    public int bomb;
    [SerializeField] private TextMeshProUGUI _goldText;
    [SerializeField] private TextMeshProUGUI _bombText;

    [Header("PickupPanel")]
    [SerializeField] private Transform _panelPosition;
    [SerializeField] private GameObject _panel;

    public enum consumableItem { Coin_Bronze, Coin_Silver, Coin_Gold, Bar_Silver, Bar_Gold };
    public enum accessoriesItem { Accessories1, Accessories2 };
    public enum weaponItem { Weapon1, Weapon2 };

    public CharacterStats playerStats;

    public GameObject[] ConsumableItemList;
    public List<GameObject> AccessoriesItemList;

    public Transform test;

    public GameObject GetConsumableItem(int index)
    {
        return Instantiate(ConsumableItemList[index], transform);
    }

    public GameObject GetAccessoriesItem(int index)
    {
        return Instantiate(AccessoriesItemList[index], transform);
    }

    private IItemDrop testDrop = new ConsumableItemDrop();
    private IItemDrop testDrop2 = new AccessoriesItemDrop();

    private void Update()
    {
        _goldText.text = gold.ToString().PadLeft(3, '0');
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

    IEnumerator DropTest2()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            testDrop2.ItemDrop(test);
        }
    }

    public void NoticeOfPanel(ItemData itemData)
    {
        GameObject pickupPanel = Instantiate(_panel, _panelPosition);
        pickupPanel.GetComponent<PickupPanel>().Set(itemData);
        
        Destroy(pickupPanel, 1.5f);
    }
}
