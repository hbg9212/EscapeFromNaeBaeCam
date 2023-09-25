using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemPanelManager : MonoBehaviour
{   
    [SerializeField] private TextMeshProUGUI _moneyText;
    [SerializeField] private TextMeshProUGUI _bombText;

    private ItemManager itemManager;

    private void Awake()
    {
        itemManager = ItemManager.instance;
    }


    private void Update()
    {
        _moneyText.text = itemManager.money.ToString().PadLeft(3, '0');
        _bombText.text = itemManager.bomb.ToString().PadLeft(3, '0');
    }

}
