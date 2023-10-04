 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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
        foreach (Pool obj in pools) {
            if (obj.tag == tag) {
                target = obj.prefab;
                return target;
            }
        }
        return null;
    }

    public void ShootBullet(Vector2 startPosition, Vector2 direction, RangedAttackData attackData) {
        GameObject obj = Instantiate(FindFromPool(attackData.bulletNameTag));
        if (obj != null) {
            obj.transform.position = startPosition;
            RangedAttackCotroller attackCotroller = obj.GetComponent<RangedAttackCotroller>();
            attackCotroller.InitializeAttack(direction, attackData, obj);
        }
    }
    
    public void ShootSkillBullet(Vector2 startPosition, Vector2 direction, RangedSkillData skillData) {
        GameObject obj = Instantiate(FindFromPool(skillData.bulletNameTag));
        if (obj != null) {
            obj.transform.position = startPosition;
            RangedSkillCotroller skillCotroller = obj.GetComponent<RangedSkillCotroller>();
            skillCotroller.InitializeSkillAttack(direction, skillData, obj);
        }
    }
}
