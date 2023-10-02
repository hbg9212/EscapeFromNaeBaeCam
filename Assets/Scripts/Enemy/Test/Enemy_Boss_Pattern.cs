using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Boss_Pattern : MonoBehaviour
{
    [SerializeField] private List<GameObject> Drones;
    [SerializeField] private Animator animator;

    private const string attack1 = "Attack_1";
    private const string attack2 = "Attack_2";

    

    void Start()
    {
        animator = GetComponent<Animator>();
        Invoke(nameof(Attack_1), 3.0f);
        Invoke(nameof(Attack_2), 6.0f);
    }
    public void Attack_1()
    {
        animator.SetTrigger(attack1);
    }
    public void Attack_2()
    {
        animator.SetTrigger(attack2);
    }
}
