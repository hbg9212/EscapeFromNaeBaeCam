using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    private CharacterController _controller;

    [SerializeField] private Transform projectileSpawnPosition;
    private Vector2 _aimDirection = Vector2.right;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    void Start()
    {
        _controller.OnAttackEvent += OnShoot;
        _controller.OnLookEvent += OnAim;
    }

    private void OnAim(Vector2 newAimDirection)
    {
        _aimDirection = newAimDirection;
    }

    private void OnShoot(AttackSo attackSo)
    {
        Transform bullet = PoolManager.I.Get((int)PoolManager.PrefabId.SnowBall).transform;
        bullet.position = projectileSpawnPosition.position;
        bullet.GetComponent<SnowBall>().Init(15f, _aimDirection.normalized);
    }
}

