using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class AnimationController : MonoBehaviour
{
    private static readonly int IsWalking = Animator.StringToHash("IsWalking");
    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int SkillAttack = Animator.StringToHash("SkillAttack");
    private static readonly int IsHit = Animator.StringToHash("IsHit");
    private static readonly int IsRoll = Animator.StringToHash("IsRoll");
    private static readonly int Ranged = Animator.StringToHash("Ranged");

    private HealthSystem _healthSystem;
    protected CharacterController controller;
    protected Rigidbody2D rigidbody;
    protected Animator animator;

    private void Awake()
    {
        _healthSystem = GetComponent<HealthSystem>();
        animator = GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        controller.OnAttackEvent += Attacking;
        controller.OnSkillEvent += SkillAttacking;
        controller.OnMoveEvent += Move;
        controller.OnRollEvent += Roll;
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
        if (obj.ranged == true) {
            animator.SetBool(Ranged, true);
        } else {
            animator.SetBool(Ranged, false);
        }
        animator.SetTrigger(Attack);
    }
    
    private void SkillAttacking(SkillSO obj)
    {
        if (obj.ranged == true) {
            animator.SetBool(Ranged, true);
        } else {
            animator.SetBool(Ranged, false);
        }
        animator.SetTrigger(SkillAttack);
    }

    private void Hit()
    {
        animator.SetBool(IsHit, true);
    }

    private void Roll()
    {
        if(rigidbody.velocity != Vector2.zero)
            animator.SetTrigger(IsRoll);
    }

    public void InvincibilityEnd()
    {
        animator.SetBool(IsHit, false);
        animator.SetBool(IsRoll, false);
    }
}
