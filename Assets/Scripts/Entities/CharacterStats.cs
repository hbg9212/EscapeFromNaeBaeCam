using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum StatsChangeType { 
    Add,
    Multiple,
    Override
}
public class CharacterStats : MonoBehaviour
{
    public StatsChangeType type;
    [Range(1, 6)] public int maxHealth;
    [Range(1f, 20f)] public float speed;

    
}
