using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class RangeEnemyContreoller : EnemyController
{
    [SerializeField] private float followRange;
    [SerializeField] private float shootRange;

    protected override void Awake()
    {
        base.Awake();
    }

    protected void FixedUpdate()
    {
        float distance = DistanceToTarget();
        Vector2 direction = DirectionToTarget();

        IsAttacking = false;
        if (distance <= followRange) {
            if (distance <= shootRange) {
                int layerMaskTarget = Stats.CurrentStats.attackSO.target;
                RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 11f,
                    (1 << LayerMask.NameToLayer("Level")) | layerMaskTarget);
                if (hit.collider != null && layerMaskTarget == (layerMaskTarget | (1 << hit.collider.gameObject.layer))) {
                    CallLookEvent(direction);
                    CallMoveEvent(Vector2.zero);
                    IsAttacking = true;
                } else {
                    CallMoveEvent(direction);
                }
            } else {
                CallMoveEvent(direction);
            }
        } else {
            CallMoveEvent(direction);
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}