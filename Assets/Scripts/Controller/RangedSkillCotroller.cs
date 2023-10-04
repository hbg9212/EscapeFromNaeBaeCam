using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RangedSkillCotroller : MonoBehaviour
{
    [SerializeField] private LayerMask levelCollisionLayer;

    private RangedSkillData _skillData;
    private float _currentDuration;
    private Vector2 _direction;
    private bool _isReady;

    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;

    private readonly int OnFx = Animator.StringToHash("OnFx");
    private Animator animator;
    public bool fxOnDestroy = true;

    private bool isDestroyProjectile = false;

    private void Awake() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update() {
        if (!_isReady)
            return;

        _currentDuration += Time.deltaTime;
        if (_currentDuration > _skillData.duration) {
            DestroyProjectile(transform.position, false);
        }

        if (!isDestroyProjectile)
            _rigidbody.velocity = _direction * _skillData.speed;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (levelCollisionLayer.value == (levelCollisionLayer.value | (1 << collision.gameObject.layer))) {
            DestroyProjectile(collision.ClosestPoint(transform.position) - _direction * .2f, fxOnDestroy);
        } else if (_skillData.target.value == (_skillData.target.value | (1 << collision.gameObject.layer))) {
            HealthSystem healthSystem = collision.GetComponent<HealthSystem>();
            if (healthSystem != null) {
                healthSystem.ChangeHealth(-_skillData.power);
                if (_skillData.isOnKnockback) {
                    Movement movement = collision.GetComponent<Movement>();
                    if (movement != null) {
                        movement.ApplyKnockback(transform, _skillData.knockbackPower, _skillData.knockbackTime);
                    }
                }
            }
            DestroyProjectile(collision.ClosestPoint(transform.position), fxOnDestroy);
        }
    }

    public void InitializeSkillAttack(Vector2 direction, RangedSkillData skillData, GameObject obj) {
        _skillData = skillData;
        _direction = direction;

        UpdateSkillProjectilSprite();
        _currentDuration = 0;
        _spriteRenderer.color = skillData.projectileColor;

        transform.right = _direction;
        _isReady = true;
    }

    private void UpdateSkillProjectilSprite() {
        transform.localScale = Vector3.one * _skillData.size;
    }

    private void DestroyProjectile(Vector3 position, bool createFx) {
        if (createFx && animator != null) {
            isDestroyProjectile = true;
            _rigidbody.velocity = Vector3.zero;
            animator.SetBool(OnFx, true);
        } else {
            Destroy(gameObject);
        }
    }
}
