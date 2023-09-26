using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DefaultAttackData", menuName = "TopDownController/Attacks/Default", order = 0)]
public class AttackSo : ScriptableObject
{
    [Header("Atttack Info")]
    public float size;
    public float delay;
    public float power;
    public float speed;
    public LayerMask target;
}
