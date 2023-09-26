using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PickupPanel : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _description;

    public void Set(ItemData itemData)
    {
        _icon.sprite = itemData.icon;
        _description.text = $"{itemData.displayName} {itemData.description}";
    }
}
