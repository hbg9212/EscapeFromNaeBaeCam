using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class RangeEnemyContreoller : EnemyController
{
    [SerializeField] private float followRange = 15f;
    [SerializeField] private float shootRange = 10f;
    [SerializeField] private SpriteRenderer characterRenderer;
    [SerializeField] private Animator animator;
    private Vector2 direction = Vector2.zero;

    private bool isAttackingCoroutineRunning = false;

    protected override void Awake()
    {
        base.Awake();
        IsAttacking = false;
    }
    protected override void Update()
    {
        if (Stats.CurrentStats.attackSO == null)
            return;

        if (_timeSinceLastAttack <= Stats.CurrentStats.attackSO.delay)
        {
            _timeSinceLastAttack += Time.deltaTime;
        }

        if (!IsAttacking && _timeSinceLastAttack > Stats.CurrentStats.attackSO.delay)
        {
            // 시작한 코루틴을 중복 실행하지 않도록 체크
            if (!isAttackingCoroutineRunning)
            {
                StartCoroutine(AttackCoroutine());
            }
        }
    }

    protected void FixedUpdate()
    {
        float distance = DistanceToTarget();
        Vector2 direction = DirectionToTarget();

        if (distance <= followRange)
        {
            if (distance <= shootRange)
            {
                CallLookEvent(direction);
                CallMoveEvent(Vector2.zero);
            }
            else
            {
                if(!IsAttacking)
                CallMoveEvent(direction);
            }
        }
        else
        {
            CallMoveEvent(direction);
        }
    }

    private IEnumerator AttackCoroutine()
    {
        animator.SetTrigger("Fire");
        IsAttacking = true;
        CallAttackEvent(Stats.CurrentStats.attackSO);
        yield return new WaitForSeconds(1.0f);
        IsAttacking = false;
        isAttackingCoroutineRunning = false;
        animator.SetTrigger("Idle");
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

    private void AttackFinish()
    {
        IsAttacking = false;
    }
}