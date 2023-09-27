using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private CharacterController _controller;
    private CharacterStatsHandler _stats;
    private AnimationController _anim;

    private Vector2 _movementDirection = Vector2.zero;
    private Rigidbody2D _rigidbody;

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
    }

    private void Move(Vector2 direction)
    {
        _movementDirection = direction;
    }

    private void ApplyMovment(Vector2 direction)
    {
        direction = direction * _stats.CurrentStats.speed;
        _rigidbody.velocity = direction;
    }

    private void Dodge()
    {
        Debug.Log(_movementDirection);
        if (_movementDirection != Vector2.zero && !(_controller.IsRolling))
        {
            //_controller.dodgeVec = _movementDirection;
            _stats.CurrentStats.speed *= 2;
            //anim.SetTrigger("doDodge");
            _controller.IsRolling = true;
            Invoke("DodgeOut", 0.5f);
        }
    }

    private void DodgeOut()
    {
        _stats.CurrentStats.speed *= 0.5f;
        _controller.IsRolling = false;
        _controller.dodgeVec = Vector2.zero;
        _anim.InvincibilityEnd();
        Move(Vector2.zero);
    }

}
