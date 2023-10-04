using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RangedSkillData", menuName = "TopDownController/Skills/Ranged", order = 1)]
public class RangedSkillData : SkillSO
{
    [Header("Ranged Skill Data")]
    public string bulletNameTag;
    public float duration;
    public float spread;
    public int numberOfProjectilesPerShot;
    public float multipleProjectilesAngle;
    public bool unDestroy;
    public Color projectileColor;
}
