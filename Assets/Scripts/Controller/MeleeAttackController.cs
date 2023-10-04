using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeleeAttackController : MonoBehaviour
{
    private CharacterController _controller;

    [SerializeField] private Transform meleeAttackPosition;
    
    private Vector2 _aimDirection = Vector2.right;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void Start()
    {
        _controller.OnAttackEvent += OnAttack;
        _controller.OnLookEvent += OnAim;
    }

    private void OnAim(Vector2 newAimDirection)
    {
        _aimDirection = newAimDirection;
    }

    private void OnAttack(AttackSO attackSO)
    {
        MeleeAttackData meleeAttackData = attackSO as MeleeAttackData;
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(meleeAttackPosition.position, meleeAttackData.attackRange, 0);
        foreach (Collider2D collider in collider2Ds)
        {
            if (meleeAttackData.target.value == (meleeAttackData.target.value | (1 << collider.gameObject.layer)))
            {
                HealthSystem healthSystem = collider.GetComponent<HealthSystem>();
                if (healthSystem != null)
                {
                    healthSystem.ChangeHealth(-meleeAttackData.power);
                    if (meleeAttackData.isOnKnockback)
                    {
                        Movement movement = collider.GetComponent<Movement>();
                        if (movement != null)
                        {
                            movement.ApplyKnockback(transform, meleeAttackData.knockbackPower, meleeAttackData.knockbackTime);
                        }
                    }
                }
            }
        }
    }
}