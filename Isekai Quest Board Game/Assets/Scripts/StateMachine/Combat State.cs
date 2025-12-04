using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatState : State
{
    //public GameObject slimeSpit;
    public Transform spitSpawn;
    public float attackCooldown = 1.5f;

    private float cooldownTimer;

    public override void Enter()
    {
        cooldownTimer -= Time.deltaTime;

        rb.velocity = Vector2.zero;
        anim.SetBool("Attacking", true);
        anim.SetBool("Moving", false);
    }

    public override void Do()
    {

    }

    /*public void FireProjectile()
    {
        GameObject spit = GameObject.Instantiate(SlimeSpit, projectileSpawn.position, Quaternion.identity);
    }*/

    public override void Exit()
    {

    }
}
