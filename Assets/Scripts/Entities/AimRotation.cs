using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimRotation : MonoBehaviour
{
    private SpriteRenderer characterRenderer;
    [SerializeField] private SpriteRenderer armRenderer;
    [SerializeField] private Transform armPivot;

    private CharacterController _controller;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        characterRenderer = GetComponent<SpriteRenderer>();
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
        armPivot.rotation = Quaternion.Euler(0, 0, rotZ);
        
        // ���콺 ��ġ�� ���� ĳ���� ��������Ʈ ������ ��ȯ
        if (Mathf.Abs(rotZ) > 90f)
        {
            armRenderer.flipY = true;
            characterRenderer.flipX = true;
        }
        else
        {
            armRenderer.flipY = false;
            characterRenderer.flipX = false;
        }
    }
}
