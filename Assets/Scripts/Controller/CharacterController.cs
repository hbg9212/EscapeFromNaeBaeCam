using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public event Action<Vector2> OnMoveEvent;
    public event Action<Vector2> OnLookEvent;

    public event Action<bool> OnInvenEvent;
    public event Action<AttackSO> OnAttackEvent;

    public event Action OnRollEvent;

    protected float _timeSinceLastAttack = float.MaxValue;
    protected CharacterStatsHandler Stats { get; private set; }
    protected bool IsAttacking { get; set; }
    public bool IsRolling { get; set; }

    //회피도중 방향전환되지않게하기위한 변수.
    public Vector2 dodgeVec;

    protected virtual void Awake() {
        Stats = GetComponent<CharacterStatsHandler>();
    }

    protected virtual void Update()
    {
        HandleAttackDelay();
    }

    private void HandleAttackDelay()
    {
        if (Stats.CurrentStats.attackSO == null)
            return;

        if(_timeSinceLastAttack <= Stats.CurrentStats.attackSO.delay)
        {
            _timeSinceLastAttack += Time.deltaTime;
        }
        
        if(IsAttacking && _timeSinceLastAttack > Stats.CurrentStats.attackSO.delay)
        {
            _timeSinceLastAttack = 0;
            CallAttackEvent(Stats.CurrentStats.attackSO);
        }
    }

    public void CallMoveEvent(Vector2 direction)
    {
        OnMoveEvent?.Invoke(direction);
    }
    
    public void CallLookEvent(Vector2 direction)
    {
        OnLookEvent?.Invoke(direction);
    }

    public void CallAttackEvent(AttackSO attackSO)
    {
        OnAttackEvent?.Invoke(attackSO);
    }

    public void CallRollEvent()
    {
        OnRollEvent?.Invoke();
    }
    
    public void CallInven(bool IsInven)
    {
        OnInvenEvent?.Invoke(IsInven);
    }

    protected virtual void OnDestroy()
    {

    }
}
