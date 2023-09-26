using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatsChangeType
{
    Add,
    Multiple,
    Override,
}

[Serializable]
public class CharacterStats
{
    public StatsChangeType type;
    [Range(1, 20)] public int maxHealth;
    [Range(1f, 20f)] public float speed;

    public AttackSO attackSO;
}
