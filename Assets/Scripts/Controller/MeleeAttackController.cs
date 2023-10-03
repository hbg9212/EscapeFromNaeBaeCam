using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeleeAttackController : MonoBehaviour
{
    private CharacterController _controller;

    [SerializeField] private Transform meleeRangePosition;
    private Vector2 _aimDirection = Vector2.right;

    private AttackSO _attackData;
    private SkillSO _skillData;
    //private TrailRenderer _trailRenderer;
    //public bool fxOnDestroy = true;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    void Start()
    {
        _controller.OnAttackEvent += OnAttack;
        _controller.OnSkillEvent += OnSkillAttack;
        _controller.OnLookEvent += OnAim;
    }

    private void OnAim(Vector2 newAimDirection)
    {
        _aimDirection = newAimDirection;
    }

    private void OnAttack(AttackSO attackSO)
    {
        MeleeAttackData meleeAttackData = attackSO as MeleeAttackData;
        //OnDrawGizmos(meleeAttackData.attackRange);
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(meleeRangePosition.position, meleeAttackData.attackRange, 0);
        foreach (Collider2D collider in collider2Ds)
        {
            if (attackSO.target.value == (attackSO.target.value | (1 << collider.gameObject.layer)))
            {
                HealthSystem healthSystem = collider.GetComponent<HealthSystem>();
                if (healthSystem != null)
                {
                    healthSystem.ChangeHealth(-attackSO.power);
                    if (attackSO.isOnKnockback)
                    {
                        Movement movement = collider.GetComponent<Movement>();
                        if (movement != null)
                        {
                            movement.ApplyKnockback(transform, attackSO.knockbackPower, attackSO.knockbackTime);
                        }
                    }
                }
            }
        }
    }

    private void OnSkillAttack(SkillSO skillSO)
    {
        MeleeSkillData meleeAttackData = skillSO as MeleeSkillData;
    }

    //private void OnDrawGizmos(Vector2 range) {
    //    Gizmos.color = Color.green;
    //    Gizmos.DrawWireCube(meleeRangePosition.position, range);
    //}
}