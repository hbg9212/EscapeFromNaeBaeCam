using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : CharacterController
{
    private Camera _camera;

    //ȸ�ǵ��� ������ȯ�����ʰ��ϱ����� ����.
    Vector3 dodgeVec;

    private void Awake()
    {
        _camera = Camera.main;
    }

    public void OnMove(InputValue value)
    {
        Vector2 moveInput;
        if (IsRolling)
        {
            moveInput = dodgeVec;
        }
        else
        {
            moveInput = value.Get<Vector2>().normalized;
        }
        CallMoveEvent(moveInput);
    }

    public void OnLook(InputValue value)
    {
        Vector2 newAim = value.Get<Vector2>();
        Vector2 worldPos = _camera.ScreenToWorldPoint(newAim);
        newAim = (worldPos - (Vector2)transform.position).normalized;

        if (newAim.magnitude >= 0.9f)
        {
            CallLookEvent(newAim);
        }
    }

    public void OnFire(InputValue value)
    {
        IsAttacking = value.isPressed;
    }

    public void OnRoll(InputValue value)
    {
        IsRolling = value.isPressed;
    }

    
    //void Dodge()
    //{
    //    // �����϶� jumpŰ������ ������, ������ ȸ�� ���� �ϳ��� �ൿ�ϱ����ؼ�
    //    if ( != Vector3.zero && !IsRolling)
    //    {
    //        //������ ���� �����̴� ��������.
    //        dodgeVec = moveVec;
    //        speed *= 2;
    //        anim.SetTrigger("doDodge");
    //        IsRolling = true;
    //        //�ð��� �ξ� ȸ�������� ���ƿ���
    //        Invoke("DodgeOut", 0.5f);
    //    }
    //}

    //void DodgeOut()
    //{   
    //    //�ӵ� ������� ���ƿ���
    //    speed *= 0.5f;
    //    IsRolling = false;
    //}
}
