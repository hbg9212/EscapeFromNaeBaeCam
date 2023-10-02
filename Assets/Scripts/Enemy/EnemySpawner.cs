using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private const string Player = "Player";

    [SerializeField] private GameObject Target;
    [SerializeField] private GameObject[] EnemyPrefabs;
    [SerializeField] private GameObject MainSprite;
    [SerializeField] private GameObject Collider;
    [SerializeField] private List<GameObject> Enemies;

    public int NumEnemyA;
    public int NumEnemyB;
    public int NumEnemyC;
    public float ColliderWidth;
    public float ColliderHeight;
    private bool first = true;
    public bool isClear= false;
    private void Start()
    {
        Collider.transform.localScale = new Vector3(ColliderWidth, ColliderHeight, 1);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (first && collision.CompareTag(Player))
        {
            EnterTarget();
            Target=collision.gameObject;
            first = false;
        }
    }

    public void SetTarget(GameObject target)
    {
        Target = target;
    }

    private void SpawnEnemies()
    {
        if (NumEnemyA != 0 && EnemyPrefabs[0] != null)
        {
            for (int i = 0; i < NumEnemyA; i++)
            {
                GameObject enemy = Instantiate(EnemyPrefabs[0], this.transform.position, Quaternion.identity);
                Enemies.Add(enemy);
            }
        }
        else
        {
            Debug.Log("NumEnenyA or EnemyPrefabs[0] is Null");
        }

        if (NumEnemyB != 0 && EnemyPrefabs[1] != null)
        {
            for (int i = 0; i < NumEnemyB; i++)
            {
                GameObject enemy = Instantiate(EnemyPrefabs[1], this.transform.position, Quaternion.identity);
                Enemies.Add(enemy);
            }
        }
        else
        {
            Debug.Log("NumEnenyB or EnemyPrefabs[1] is Null");
        }

        if (NumEnemyC != 0 && EnemyPrefabs[2] != null)
        {
            for (int i = 0; i < NumEnemyC; i++)
            {
                GameObject enemy = Instantiate(EnemyPrefabs[2], this.transform.position, Quaternion.identity);
                Enemies.Add(enemy);
            }
        }
        else
        {
            Debug.Log("NumEnenyC or EnemyPrefabs[2] is Null");
        }
    }

    private void SetMainSprite()
    {
        MainSprite.SetActive(false);
    }

    private void EnterTarget()
    {
        MainSprite.SetActive(true);
        Invoke(nameof(SpawnEnemies), 0.3f);
        Invoke(nameof(SetMainSprite), 0.5f);
    }

    public void RemoveFromList(GameObject gameObject)
    {
        Enemies.Remove(gameObject);
        CheckListIsZero();
    }

    public void CheckListIsZero()
    {
        if(Enemies.Count== 0)
        {
            isClear= true;
            return;
        }
    }
}
