using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DefaultSkillData", menuName = "TopDownController/Skills/Default", order = 0)]
public class SkillSO : ScriptableObject {
    [Header("Skill Type")]
    public bool ranged;

    [Header("Skill Info")]
    public float size;
    public float delay;
    public float power;
    public float speed;
    public LayerMask target;
    public Sprite icon;

    [Header("Knock Back Info")]
    public bool isOnKnockback;
    public float knockbackPower;
    public float knockbackTime;
}

