using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatState : State
{
    public AggroState aggro;
    //public GameObject slimeSpit;
    public Transform attackPoint;
    public float attackCooldown = 1.5f;
    public float attackRange = 2.5f;

    private float cooldownTimer;

    public override void Enter()
    {
        cooldownTimer -= Time.deltaTime;

        rb.velocity = Vector2.zero;
        anim.SetBool("Attacking", true);
    }

    public override void Do()
    {
        
    }

    void Attack()
    {
        anim.SetBool("Attacking", true);

        if (cooldownTimer <= 0f)
        {
            cooldownTimer = attackCooldown;
        }
    }

    public override void Exit()
    {

    }
}
