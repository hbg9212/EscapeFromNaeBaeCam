using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class EnemyController : CharacterController
{
    [SerializeField] protected GameObject Target;
    [SerializeField] protected EnemySpawner enemySpawner;
  

    
    public void SetTarget(GameObject target)
    {
        Target = target;
    }

    public void SetSpawner(EnemySpawner enemySpawner)
    {
        this.enemySpawner = enemySpawner;
    }

    protected virtual void OnDestroy()
    {
        if(enemySpawner != null) 
        enemySpawner.RemoveFromList(this.gameObject);
    }

    protected float DistanceToTarget()
    {
        return Vector3.Distance(transform.position, Target.transform.position);
    }

    protected Vector2 DirectionToTarget()
    {
        return (Target.transform.position - transform.position).normalized;
    }
} 
