using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class DroneCannon : MonoBehaviour
{
  
    [SerializeField] private RangedAttackData rangedAttackData;

    [SerializeField] private Transform projectileSpawnPositionStart;
    [SerializeField] private Transform projectileSpawnPositionEnd;
    [SerializeField] private GameObject BulletPrefab;

    private Vector3 dirction = Vector3.right;
    private ObjectPoolManager ObjectPoolManager;

    private void Awake()
    {
        ObjectPoolManager = ObjectPoolManager.instance;
    }

    public void CreateProjectile()
    {
        GameObject gameObject = Instantiate(BulletPrefab, projectileSpawnPositionEnd.position, Quaternion.identity);
        RangedAttackCotroller rangedAttackCotroller= gameObject.GetComponent<RangedAttackCotroller>();

        float angle = transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
        dirction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

        if (transform.localScale.x < 0)
            dirction.x *= -1;
        dirction *= 2;
      
        rangedAttackCotroller.InitializeAttack(dirction, rangedAttackData, this.gameObject, ObjectPoolManager);
    }

 
}
