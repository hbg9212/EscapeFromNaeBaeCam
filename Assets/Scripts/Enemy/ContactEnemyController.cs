using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactEnemyController : EnemyController
{
    [SerializeField][Range(0f, 100f)] private float followRange;
    private bool _isCollidingWithTarget;
    private Vector2 direction = Vector2.zero;
    [SerializeField] private SpriteRenderer characterRenderer;
    private Rigidbody2D rb;
    public float speed = 100.0f;

    protected void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected void FixedUpdate()
    {
         direction = Vector2.zero;
        if (DistanceToTarget() < followRange)
        {
            direction = DirectionToTarget();
        }
        rb.velocity = direction * speed * Time.fixedDeltaTime;
        CallMoveEvent(direction);
        Rotate(direction);
    }

    private void Rotate(Vector2 direction)
    {
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        characterRenderer.flipX = Mathf.Abs(rotZ) > 90f;
    }
}