using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RangedAttackCotroller : MonoBehaviour
{
    [SerializeField] private LayerMask levelCollisionLayer;

    private RangedAttackData _attackData;
    private RangedSkillData _skillData;
    private float _currentDuration;
    private Vector2 _direction;
    private bool _isReady;

    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;
    //private TrailRenderer _trailRenderer;

    //public bool fxOnDestroy = true;

    private void Awake() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
        //_trailRenderer = GetComponent<TrailRenderer>();
    }

    private void Update() {
        if (!_isReady)
            return;

        if (_skillData == null) {
            _currentDuration += Time.deltaTime;
            if (_currentDuration > _attackData.duration) {
                DestroyProjectile(transform.position, false);
            }

            _rigidbody.velocity = _direction * _attackData.speed;
        } else if (_skillData != null) { 
            _currentDuration += Time.deltaTime;
            if (_currentDuration > _skillData.duration) {
                DestroyProjectile(transform.position, false);
            }
            _rigidbody.velocity = _direction * _skillData.speed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (levelCollisionLayer.value == (levelCollisionLayer.value | (1 << collision.gameObject.layer))) {
            DestroyProjectile(collision.ClosestPoint(transform.position) - _direction * .2f, false);
        } else if (_skillData == null && _attackData.target.value == (_attackData.target.value | (1 << collision.gameObject.layer))) {
            HealthSystem healthSystem = collision.GetComponent<HealthSystem>();
            if (healthSystem != null) {
                healthSystem.ChangeHealth(-_attackData.power);
                if (_attackData.isOnKnockback) {
                    Movement movement = collision.GetComponent<Movement>();
                    if (movement != null) {
                        movement.ApplyKnockback(transform, _attackData.knockbackPower, _attackData.knockbackTime);
                    }
                }
            }
            DestroyProjectile(collision.ClosestPoint(transform.position), false);
        } else if (_skillData != null && _skillData.target.value == (_skillData.target.value | (1 << collision.gameObject.layer))) {
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
            DestroyProjectile(collision.ClosestPoint(transform.position), false);
        }
    }

    public void InitializeAttack(Vector2 direction, RangedAttackData attackData, GameObject obj) {
        _attackData = attackData;
        _direction = direction;

        //_trailRenderer.Clear();
        UpdateProjectilSprite();
        _currentDuration = 0;
        _spriteRenderer.color = attackData.projectileColor;

        transform.right = _direction;
        _isReady = true;
    }

    public void InitializeSkillAttack(Vector2 direction, RangedSkillData skillData, GameObject obj) {
        _skillData = skillData;
        _direction = direction;

        //_trailRenderer.Clear();
        UpdateSkillProjectilSprite();
        _currentDuration = 0;
        _spriteRenderer.color = skillData.projectileColor;

        transform.right = _direction;
        _isReady = true;
    }

    private void UpdateProjectilSprite() {
        transform.localScale = Vector3.one * _attackData.size;
    }
    private void UpdateSkillProjectilSprite() {
        transform.localScale = Vector3.one * _skillData.size;
    }

    private void DestroyProjectile(Vector3 position, bool createFx) {
        if (createFx) {

        }
        Destroy(gameObject);
    }
}
