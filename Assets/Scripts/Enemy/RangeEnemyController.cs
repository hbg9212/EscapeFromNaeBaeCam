using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class RangeEnemyContreoller : EnemyController
{
    [SerializeField][Range(0f, 100f)] private float followRange;
    private bool _isCollidingWithTarget;
    private Vector2 direction = Vector2.zero;
    [SerializeField] private SpriteRenderer characterRenderer;

    protected override void Awake()
    {
        base.Awake();
    }

    protected void FixedUpdate()
    {
        direction = DirectionToTarget();

        direction = Vector2.zero;
        if (DistanceToTarget() < followRange)
        {
            direction = DirectionToTarget();
        }
        CallMoveEvent(direction);
        Rotate(direction);

    }

    private void Rotate(Vector2 direction)
    {
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        characterRenderer.flipX = Mathf.Abs(rotZ) > 90f;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}