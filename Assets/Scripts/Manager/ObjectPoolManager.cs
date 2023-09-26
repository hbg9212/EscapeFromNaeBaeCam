using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager instance;

    [System.Serializable]
    public struct Pool {
        public string tag;
        public GameObject prefab;
    }

    public List<Pool> pools;

    void Awake()
    {
        instance = this;
    }

    public GameObject FindFromPool(string tag)
    {
        GameObject target;
        foreach (Pool pool in pools) { 
            if (pool.tag == tag) {
                target = pool.prefab;
                return target;
            }
        }
        return null;
    }

    public void ShootBullet(Vector2 startPosition, Vector2 direction, RangedAttackData attackData) {
        GameObject obj = Instantiate(FindFromPool(attackData.bulletNameTag));

        obj.transform.position = startPosition;
        RangedAttackCotroller attackCotroller = obj.GetComponent<RangedAttackCotroller>();
        attackCotroller.InitializeAttack(direction, attackData, obj, this);
        //obj.SetActive(true);
    }
}
