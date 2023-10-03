using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MeleeAttackData", menuName = "TopDownController/Attacks/Melee", order = 2)]
public class MeleeAttackData : AttackSO
{
    [Header("Melee Attack Data")]
    public Vector2 attackRange;
    public Color projectileColor;
}
