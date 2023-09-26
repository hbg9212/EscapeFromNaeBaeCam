using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class RangeEnemyContreoller : EnemyController
{
    [SerializeField] private float followRange = 15f;
    [SerializeField] private float shootRange = 10f;
    [SerializeField] private SpriteRenderer characterRenderer;
    [SerializeField] private GameObject BulletPrefab;
    [SerializeField] private Transform BulletSpawnPoint;
    private Vector2 direction = Vector2.zero;
    protected override void Awake()
    {
        base.Awake();
        OnAttackEvent += ShootBullet;
    }
    protected void FixedUpdate()
    {
        float distance = DistanceToTarget();
        Vector2 direction = DirectionToTarget();

        IsAttacking = true;
        if (distance <= followRange)
        {
            if (distance <= shootRange)
            {
                CallLookEvent(direction);
                CallMoveEvent(Vector2.zero);
            }
            else
            {
                CallMoveEvent(direction);
            }
        }
        else
        {
            CallMoveEvent(direction);
        }
    }
  

    private void Rotate(Vector2 direction)
    {
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        characterRenderer.flipX = Mathf.Abs(rotZ) > 90f;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    private void ShootBullet(AttackSO attackSo)
    {
        GameObject bullet = Instantiate(BulletPrefab, BulletSpawnPoint.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().Shoot(Target.transform);
    }
}