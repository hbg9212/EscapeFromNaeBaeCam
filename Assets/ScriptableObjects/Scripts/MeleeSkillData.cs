using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MeleeSkillData", menuName = "TopDownController/Skills/Melee", order = 2)]
public class MeleeSkillData : SkillSO
{
    [Header("Melee Skill Data")]
    public Vector2 attackRange;
    public Color projectileColor;
}
