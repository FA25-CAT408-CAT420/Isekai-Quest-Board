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
        float distance = Vector2.Distance(core.transform.position, aggro.target.position);

        if (distance <= attackRange)
        {
            rb.velocity = Vector2.zero;
            anim.SetBool("Moving", false);
            Attack();
            return;
        }
    }

    void Attack()
    {
        anim.SetBool("Attacking", true);

        if (cooldownTimer <= 0f)
        {
            cooldownTimer = attackCooldown;
        }
    }

    /*public void FireProjectile()
    {
        GameObject spit = GameObject.Instantiate(SlimeSpit, projectileSpawn.position, Quaternion.identity);
        float facing = core.transform.localScale.x;
        Vector2 direction = new Vector2(facing, 0);
    }*/

    public override void Exit()
    {

    }
}
