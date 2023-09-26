using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody2D rb;
    private Vector3 direction;
    public float speed= 10.0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Shoot(Transform Target)
    {
        direction = Target.position - this.transform.position;
        direction = Vector3.Normalize(direction);
    }

    private void FixedUpdate()
    {
        rb.velocity = direction * speed;
    }
}
