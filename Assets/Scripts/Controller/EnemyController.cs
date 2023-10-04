using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : CharacterController
{
    protected Transform ClosestTarget { get; private set; }
    protected EnemySpawner _enemySpawner;
    protected HealthSystem healthSystem;

    // 임시로 추가
    protected SpriteRenderer _spriteRenderer;
    protected override void Awake()
    {
        base.Awake();
        healthSystem = GetComponent<HealthSystem>();
        //healthSystem.OnDeath += RemoveFromEnemySpawner;
    }

    protected virtual void Start()
    {
        ClosestTarget = GameManager.instance.Player;
    }

    protected virtual void FixedUpdate()
    {

    }

    protected float DistanceToTarget()
    {
        return Vector3.Distance(transform.position, ClosestTarget.position);
    }

    protected Vector2 DirectionToTarget()
    {
        return (ClosestTarget.position - transform.position).normalized;
    }

    public void SetSpawner(EnemySpawner spawner)
    {
        _enemySpawner = spawner;
    }

    //private void RemoveFromEnemySpawner()
    //{
    //    _enemySpawner.RemoveFromList(gameObject);
    //}
} 
