using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorRender : MonoBehaviour
{
    public static readonly int HASH_ATTACK = Animator.StringToHash("Attack");
    public static readonly int HASH_WALK = Animator.StringToHash("Walk");
    public static readonly int HASH_DEATH = Animator.StringToHash("DEATH");

    public Actor actor;
    public Animator animator;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void Attack()
    {
        animator.SetTrigger(HASH_ATTACK);
    }
}
