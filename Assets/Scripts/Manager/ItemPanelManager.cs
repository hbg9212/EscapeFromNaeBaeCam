using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemPanelManager : MonoBehaviour
{   
    public static ItemPanelManager instance;


    [SerializeField] private TextMeshProUGUI _moneyText;
    [SerializeField] private TextMeshProUGUI _bombText;

    public int money;
    public int bomb;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        _moneyText.text = money.ToString().PadLeft(3, '0');
        _bombText.text = bomb.ToString().PadLeft(3, '0');
    }

}
