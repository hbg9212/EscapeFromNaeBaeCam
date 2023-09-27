using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackController : MonoBehaviour
{
    private MeleeAttackData _attackData;
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

        _rigidbody.velocity = _direction * _attackData.speed;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (_attackData.target.value == (_attackData.target.value | (1 << collision.gameObject.layer))) {
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
        }
    }

    private void UpdateProjectilSprite() {
        transform.localScale = Vector3.one * _attackData.size;
    }
}
