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
    [Description("���")] Gold
    , [Description("��ź")] Bomb
    , [Description("ü��")] Health
    , [Description("�ִ� ü��")] Maxhealth
    , [Description("�߻�ü ũ��")] Size
    , [Description("���� �ӵ�")] Delay
    , [Description("�ִ� ü��")] Power
    , [Description("�̵��ӵ�")] Speed
    , [Description("�˹� �Ŀ�")] KnockbackPower
    , [Description("�˹� ��ȿ�ð�")] KnockbackTime
    , [Description("���� �ð�")] Duration
    , [Description("�߻� ����")] Spread
    , [Description("�߻�ü ��")] NumberOfProjectilesPerShot
    , [Description("�߻�ü ����")] MultipleProjectilesAngle
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