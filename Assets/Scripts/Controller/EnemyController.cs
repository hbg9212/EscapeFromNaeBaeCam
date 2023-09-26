using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class EnemyController : MonoBehaviour
{
   [SerializeField]private GameObject Target;
    [SerializeField] private EnemySpawner enemySpawner;
    private Rigidbody2D rb;
    private Vector3 MoveDirection;
    private float direction;
    public float speed = 10;
   
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetTarget(GameObject target)
    {
        Target = target;
    }

    private void FixedUpdate()
    {
       MoveDirection= Target.transform.position- transform.position;
       direction = Mathf.Sign(MoveDirection.x);
       transform.localScale = new Vector3(direction, transform.localScale.y, transform.localScale.z);
       rb.velocity = MoveDirection * Time.fixedDeltaTime * speed;
    }

    public void SetSpawner(EnemySpawner enemySpawner)
    {
        this.enemySpawner = enemySpawner;
    }

    private void OnDestroy()
    {
        enemySpawner.RemoveFromList(this.gameObject);
    }
} 
