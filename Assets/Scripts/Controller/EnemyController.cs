using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class EnemyController : MonoBehaviour
{
    [SerializeField]private GameObject Target;
    private Rigidbody2D rb;
    private Vector3 MoveDirction;
    public float speed = 10;
    private float direction;

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
       MoveDirction= Target.transform.position- transform.position;
       direction = Mathf.Sign(MoveDirction.x);
       transform.localScale = new Vector3(direction, transform.localScale.y, transform.localScale.z);
       rb.velocity = MoveDirction * Time.fixedDeltaTime * speed;
    }

} 
