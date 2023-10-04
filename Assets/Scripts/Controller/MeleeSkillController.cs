using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeleeSkillController : MonoBehaviour
{
    private CharacterController _controller;

    [SerializeField] private Transform meleeSkillAttackPosition;
    private IEnumerator skillCoroutine;
    private float _currentDuration;
    private float _skillDuration;
    private bool onSkill;

    private Vector2 _aimDirection = Vector2.right;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void Update() {
        if (onSkill) {
            _currentDuration += Time.deltaTime;
            if (_currentDuration > _skillDuration) {
                StopCoroutine(skillCoroutine);
                onSkill = false;
            }
        }
    }

    private void Start()
    {
        _controller.OnSkillEvent += OnMeleeSkill;
        _controller.OnLookEvent += OnAim;
    }

    private void OnAim(Vector2 newAimDirection)
    {
        _aimDirection = newAimDirection;
    }

    private void OnMeleeSkill(SkillSO skillSO)
    {
        MeleeSkillData meleeSkillData = skillSO as MeleeSkillData;
        _currentDuration = 0;
        _skillDuration = meleeSkillData.duration;
        skillCoroutine = MeleeSkillAttacking(meleeSkillData);
        onSkill = true;
        StartCoroutine(skillCoroutine);
    }

    IEnumerator MeleeSkillAttacking(MeleeSkillData meleeSkillData) {
        while(true) {
            Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(meleeSkillAttackPosition.position, meleeSkillData.attackRange, 0);
            foreach (Collider2D collider in collider2Ds) {
                if (meleeSkillData.target.value == (meleeSkillData.target.value | (1 << collider.gameObject.layer))) {
                    HealthSystem healthSystem = collider.GetComponent<HealthSystem>();
                    if (healthSystem != null) {
                        healthSystem.ChangeHealth(-meleeSkillData.power);
                        if (meleeSkillData.isOnKnockback) {
                            Movement movement = collider.GetComponent<Movement>();
                            if (movement != null) {
                                movement.ApplyKnockback(transform, meleeSkillData.knockbackPower, meleeSkillData.knockbackTime);
                            }
                        }
                    }
                }
            }
            yield return null;
        }
    }
}