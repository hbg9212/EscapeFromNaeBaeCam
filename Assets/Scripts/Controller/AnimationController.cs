using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class AnimationController : MonoBehaviour
{
    private static readonly int IsWalking = Animator.StringToHash("IsWalking");
    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int IsHit = Animator.StringToHash("IsHit");
    private static readonly int IsRoll = Animator.StringToHash("IsRoll");

    private HealthSystem _healthSystem;
    protected CharacterController controller;
    protected Animator animator;

    private void Awake()
    {
        _healthSystem = GetComponent<HealthSystem>();
        animator = GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();
    }

    void Start()
    {
        controller.OnAttackEvent += Attacking;
        controller.OnMoveEvent += Move;

        if (_healthSystem != null) {
            _healthSystem.OnDamage += Hit;
            _healthSystem.OnInvincibilityEnd += InvincibilityEnd;
        }
    }

    private void Move(Vector2 obj)
    {
        animator.SetBool(IsWalking, obj.magnitude > .5f);
    }

    private void Attacking(AttackSO obj)
    {
        animator.SetTrigger(Attack);
    }

    private void Hit()
    {
        animator.SetBool(IsHit, true);
    }

    private void Roll()
    {
        animator.SetBool(IsRoll, true);
    }

    private void InvincibilityEnd()
    {
        animator.SetBool(IsHit, false);
        animator.SetBool(IsRoll, false);
    }
}
