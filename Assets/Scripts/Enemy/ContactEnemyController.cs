using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactEnemyController : EnemyController
{
    [SerializeField][Range(0f, 100f)] private float followRange;
    
    private bool _isCollidingWithTarget;
    private Vector2 direction = Vector2.zero;
    [SerializeField] private SpriteRenderer characterRenderer;

    private HealthSystem healthSystem;
    private HealthSystem _collidingTargetHealthSystem;
    private Movement _collingMovement;

    protected override void Awake()
    {
        base.Awake();
        healthSystem = GetComponent<HealthSystem>();
        healthSystem.OnDamage += OnDamage;
    }

    private void OnDamage() {
        followRange = 100f;
    }

    protected void FixedUpdate()
    {
        direction = Vector2.zero;
        if (DistanceToTarget() < followRange)
        {
            direction = DirectionToTarget();
        }
        CallMoveEvent(direction);
        Rotate(direction);

        if (_isCollidingWithTarget)
            ApplyHealthChange();
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

    private void OnTriggerEnter2D(Collider2D collision) {
        GameObject receiver = collision.gameObject;

        if (!receiver.CompareTag(base.Target.tag) && base.Target.tag != "Player")
            return;

        _collidingTargetHealthSystem = receiver.GetComponent<HealthSystem>();
        if (_collidingTargetHealthSystem != null ) 
            _isCollidingWithTarget = true;

        _collingMovement = receiver.GetComponent<Movement>();
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (!collision.CompareTag(base.Target.tag) && base.Target.tag != "Player")
            return;

        _isCollidingWithTarget = false;
    }

    private void ApplyHealthChange() {
        AttackSO attackSO = Stats.CurrentStats.attackSO;
        bool hasBeenChanged = _collidingTargetHealthSystem.ChangeHealth(-attackSO.power);
        if (attackSO.isOnKnockback && _collingMovement != null) {
            _collingMovement.ApplyKnockback(transform, attackSO.knockbackPower, attackSO.knockbackTime);
        }
    }
}