using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackController : MonoBehaviour {
    private CharacterController _controller;

    [SerializeField] private Transform meleeRangePosition;
    [SerializeField] private Vector2 meleeRange;
    private Vector2 _aimDirection = Vector2.right;

    private AttackSO _attackData;
    //private TrailRenderer _trailRenderer;
    //public bool fxOnDestroy = true;

    private void Awake() {
        _controller = GetComponent<CharacterController>();
    }

    void Start() {
        _controller.OnAttackEvent += OnAttack;
        _controller.OnLookEvent += OnAim;
    }

    private void OnAim(Vector2 newAimDirection) {
        _aimDirection = newAimDirection;
    }

    private void OnAttack(AttackSO attackSO) {
        MeleeAttackData meleeAttackData = attackSO as MeleeAttackData;
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(meleeRangePosition.position, meleeRange, 0);
        foreach (Collider2D collider in collider2Ds) {
            if (attackSO.target.value == (attackSO.target.value | (1 << collider.gameObject.layer))) {
                HealthSystem healthSystem = collider.GetComponent<HealthSystem>();
                if (healthSystem != null) {
                    healthSystem.ChangeHealth(-attackSO.power);
                    if (attackSO.isOnKnockback) {
                        Movement movement = collider.GetComponent<Movement>();
                        if (movement != null) {
                            movement.ApplyKnockback(transform, attackSO.knockbackPower, attackSO.knockbackTime);
                        }
                    }
                }
            }
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(meleeRangePosition.position, meleeRange);
    }

}
