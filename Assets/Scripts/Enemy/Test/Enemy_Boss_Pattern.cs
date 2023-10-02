using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Boss_Pattern : MonoBehaviour
{
    [SerializeField] private List<GameObject> Drones;
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform Player;

    GameManager gameManager;

    private const string Idle = "Idle";
    private const string attack1 = "Attack_1";
    private const string attack2 = "Attack_2";

    private int AttackCount = 0;
    public float moveSpeed = 5.0f;

    void Start()
    {
        gameManager = GameManager.instance;

        rb =this.GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if (Player == null)
            Player = gameManager.Player;
    
        StartCoroutine(AttackRoutine());
    }

    IEnumerator AttackRoutine()
    {
        while (true)
        {
             if(AttackCount > 5)
             {
                AttackCount = 0;
                yield return new WaitForSeconds(3.0f);
             }

             int num = Random.Range(0, 2);
             switch (num)
             {
                  case 0:
                     animator.SetTrigger(attack1);
                    break;
                  case 1:
                     animator.SetTrigger(attack2);
                     break;
                  default : 
                     break;    
             }
            AttackCount++;
            yield return null;
        }
    }
    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, Player.position);

        if (distanceToPlayer > 10f)
        {
            Vector2 direction = (Player.position - transform.position).normalized;
            rb.velocity = direction * moveSpeed;
        }
        else if(Player.position.y > this.transform.position.y && AttackCount ==5) 
        {
            this.transform.position = Player.position+Vector3.up*distanceToPlayer;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

}
