using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.TextCore.Text;

public class Movement : MonoBehaviour
{
    [SerializeField] public GameObject character;

    private CharacterController _controller;
    private CharacterStatsHandler _stats;
    private AnimationController _anim;

    private Vector2 _movementDirection = Vector2.zero;
    private Rigidbody2D _rigidbody;

    private Vector2 _knockback = Vector2.zero;
    private float knockbackDuration = 0.0f;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _stats = GetComponent<CharacterStatsHandler>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _anim = GetComponent<AnimationController>();
    }

    private void Start()
    {
        _controller.OnMoveEvent += Move;
        _controller.OnRollEvent += Dodge;
    }

    private void FixedUpdate()
    {
        ApplyMovment(_movementDirection);
        if (knockbackDuration > 0.0f) {
            knockbackDuration -= Time.fixedDeltaTime;
        }
    }

    private void Move(Vector2 direction)
    {
        _movementDirection = direction;
    }

    public void ApplyKnockback(Transform other, float power, float duration) {
        knockbackDuration = duration;
        _knockback = -(other.position - transform.position).normalized * power;
    }

    private void ApplyMovment(Vector2 direction)
    {
        direction = direction * _stats.CurrentStats.speed;
        if (knockbackDuration > 0.0f) {
            direction += _knockback;
        }
        _rigidbody.velocity = direction;
    }

    private void Dodge()
    {
        _controller._timeSinceLastRoll = 0;
        if (_rigidbody.velocity != Vector2.zero && (_controller.IsRolling))
        {
            //_controller.dodgeVec = _movementDirection;
            _stats.CurrentStats.speed *= 2;
            //anim.SetTrigger("doDodge");
            //_controller.IsRolling = true;
            character.gameObject.tag = "Rolling";
            character.gameObject.layer = LayerMask.NameToLayer("Rolling");
            Invoke("DodgeOut", 0.5f);
        }
    }

    private void DodgeOut()
    {
        Debug.Log("DodgeOut");
        _stats.CurrentStats.speed *= 0.5f;
        _controller.IsRolling = false;
        _controller.dodgeVec = Vector2.zero;
        _anim.InvincibilityEnd();
        character.gameObject.tag = "Player";
        character.gameObject.layer = LayerMask.NameToLayer("Player");
        Move(Vector2.zero);
    }

}
