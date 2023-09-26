using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private CharacterController _controller;
    private CharacterStatsHandler _stats;

    private Vector2 _movementDirection = Vector2.zero;
    private Rigidbody2D _rigidbody;

    //회피도중 방향전환되지않게하기위한 변수.
    Vector2 dodgeVec;


    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _stats = GetComponent<CharacterStatsHandler>();
        _rigidbody = GetComponent<Rigidbody2D>();
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
        if (_movementDirection != Vector2.zero && !(_controller.IsRolling))
        {
            dodgeVec = _movementDirection;
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
    }

}
