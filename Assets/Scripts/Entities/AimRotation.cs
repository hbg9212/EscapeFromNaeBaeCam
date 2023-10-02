using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimRotation : MonoBehaviour
{
    private SpriteRenderer characterRenderer;
    [SerializeField] private SpriteRenderer armRenderer;
    [SerializeField] private Transform armPivot;

    private CharacterStatsHandler _characterStatsHandler;
    private CharacterController _controller;

    private void Awake()
    {
        characterRenderer = GetComponent<SpriteRenderer>();
        _characterStatsHandler = GetComponent<CharacterStatsHandler>();
        _controller = GetComponent<CharacterController>();
    }

    void Start()
    {
        _controller.OnLookEvent += OnAim;
    }

    public void OnAim(Vector2 newAimDirection)
    {
        RotateArm(newAimDirection);
    }

    private void RotateArm(Vector2 direction)
    {
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        armRenderer.flipY = Mathf.Abs(rotZ) > 90f;
        characterRenderer.flipX = armRenderer.flipY;
        armPivot.rotation = Quaternion.Euler(0, 0, rotZ);
    }
}
