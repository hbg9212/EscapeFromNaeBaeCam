using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public enum ItemType
{
    Consumable
    , Accessories
    , Weapon
}

public enum AdditionType
{
    [Description("골드")] Gold
    , [Description("폭탄")] Bomb
    , [Description("체력")] Health
    , [Description("최대 체력")] Maxhealth
    , [Description("발사체 크기")] Size
    , [Description("연사 속도")] Delay
    , [Description("최대 체력")] Power
    , [Description("이동속도")] Speed
    , [Description("넉백 파워")] KnockbackPower
    , [Description("넉백 유효시간")] KnockbackTime
    , [Description("지속 시간")] Duration
    , [Description("발사 범위")] Spread
    , [Description("발사체 수")] NumberOfProjectilesPerShot
    , [Description("발사체 각도")] MultipleProjectilesAngle
}

[System.Serializable]
public class ItemDataAddition
{
    public AdditionType type;
    public float value;
}

[CreateAssetMenu(fileName = "Item", menuName = "Item/New Item", order = 0)]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string displayName;
    public string description;
    public ItemType type;
    public Sprite icon;
    public GameObject dropPrefab;

    [Header("Consumable")]
    public ItemDataAddition[] addition;
}