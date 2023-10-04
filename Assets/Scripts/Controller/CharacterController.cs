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
    public event Action<SkillSO> OnSkillEvent;

    public event Action OnRollEvent;

    protected float _timeSinceLastAttack = float.MaxValue;
    public float _timeSinceLastRoll = float.MaxValue;
    private float _timeSinceLastSkillAttack = float.MaxValue;
    protected CharacterStatsHandler Stats { get; private set; }
    protected bool IsAttacking { get; set; }
    protected bool IsSkillAttacking { get; set; }
    public bool IsRolling { get; set; }

    public Vector2 dodgeVec;

    protected virtual void Awake() {
        Stats = GetComponent<CharacterStatsHandler>();
    }

    protected virtual void Update()
    {
        HandleAttackDelay();
        HandleRollDelay();
        HandleSkillDelay();
    }

    private void HandleAttackDelay()
    {
        if (Stats.CurrentStats.attackSO == null || IsSkillAttacking)
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

    private void HandleRollDelay()
    {
        if (_timeSinceLastRoll <= 2.0f)
        {
            _timeSinceLastRoll += Time.deltaTime;
            IsRolling = false;
        }

        if (IsRolling && _timeSinceLastRoll > 2.0f)
        {
            Debug.Log("_timeSinceLastRoll > 2.0f");
            CallRollEvent();
        }
    }
    private void HandleSkillDelay() {
        if (Stats.CurrentStats.skillSO == null)
            return;

        if (_timeSinceLastSkillAttack <= Stats.CurrentStats.skillSO.delay) {
            _timeSinceLastSkillAttack += Time.deltaTime;
        }

        if (IsSkillAttacking && _timeSinceLastSkillAttack > Stats.CurrentStats.skillSO.delay) {
            _timeSinceLastSkillAttack = 0;
            CallSkillEvent(Stats.CurrentStats.skillSO);
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
    
    public void CallSkillEvent(SkillSO skillSO)
    {
        OnSkillEvent?.Invoke(skillSO);
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
