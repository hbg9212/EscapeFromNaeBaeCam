using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    private ObjectPoolManager _objectPoolManager;
    private CharacterController _controller;

    [SerializeField] private Transform projectileSpawnPosition;
    private Vector2 _aimDirection = Vector2.right;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    void Start()
    {
        _objectPoolManager = ObjectPoolManager.instance;
        _controller.OnAttackEvent += OnShoot;
        _controller.OnLookEvent += OnAim;
    }

    private void OnAim(Vector2 newAimDirection)
    {
        _aimDirection = newAimDirection;
    }

    private void OnShoot(AttackSO attackSO)
    {
        RangedAttackData rangedAttackData = attackSO as RangedAttackData; // ���� ����
        float projectilesAngleSpace = rangedAttackData.multipleProjectilesAngle; // �ޱ�
        int numberOfProjectilesPerShot = rangedAttackData.numberOfProjectilesPerShot; // �߻�ü ��

        float minAngle = -(numberOfProjectilesPerShot / 2) * projectilesAngleSpace + 0.5f * rangedAttackData.multipleProjectilesAngle; // �ޱ� ��ġ�� ���� ����ø�

        for (int i = 0; i < numberOfProjectilesPerShot; i++) {
            float angle = minAngle + projectilesAngleSpace * i; // �߻�ü���� �ޱ� ����
            float randomSpread = Random.Range(-rangedAttackData.spread, rangedAttackData.spread); // ź�л�
            angle += randomSpread;
            CreateProjectile(rangedAttackData, angle);
        }
    }

    private void CreateProjectile(RangedAttackData rangedAttackData, float angle) {
        _objectPoolManager.ShootBullet(
            projectileSpawnPosition.position,
            RotateVector2(_aimDirection, angle),
            rangedAttackData
            );
        //Transform bullet = PoolManager.I.Get((int)PoolManager.PrefabId.SnowBall).transform;
        //bullet.position = projectileSpawnPosition.position;
        //bullet.GetComponent<SnowBall>().Init(15f, _aimDirection.normalized);
    }

    private static Vector2 RotateVector2(Vector2 v, float degree) {
        return Quaternion.Euler(0, 0, degree) * v; // �߻�ü ���� ȸ��
    }
}

