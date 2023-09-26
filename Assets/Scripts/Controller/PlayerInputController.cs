using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : CharacterController
{
    private Camera _camera;

    //회피도중 방향전환되지않게하기위한 변수.
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
    //    // 움직일때 jump키누르면 구르기, 점프나 회피 둘중 하나만 행동하기위해서
    //    if ( != Vector3.zero && !IsRolling)
    //    {
    //        //구르는 방향 움직이는 방향으로.
    //        dodgeVec = moveVec;
    //        speed *= 2;
    //        anim.SetTrigger("doDodge");
    //        IsRolling = true;
    //        //시간차 두어 회피폼에서 돌아오기
    //        Invoke("DodgeOut", 0.5f);
    //    }
    //}

    //void DodgeOut()
    //{   
    //    //속도 원래대로 돌아오기
    //    speed *= 0.5f;
    //    IsRolling = false;
    //}
}
