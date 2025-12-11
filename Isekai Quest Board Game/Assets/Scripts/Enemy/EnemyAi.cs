using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAi : EnemyCore
{
    public PatrolState patrol;
    public AggroState aggro;
    public CombatState combat;

    void Start()
    {
        currentHealth = maxHealth;
        SetUpInstances();
        Set(patrol);
    }

    void Update()
    {
        if (state == patrol) 
        {
            aggro.CheckForTarget();

            if (aggro.target != null)
            {
                Set(aggro);
            }
        }

        state.DoBranch();
    }

    void FixedUpdate()
    {
          state.FixedDoBranch();
    }

    void OnCollisionEnter2D (Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);
            other.gameObject.GetComponent<PlayerMovement>().StopMovementCoroutine();
            other.gameObject.GetComponent<PlayerKnockback>().ApplyKnockback(transform.position);
        }
    }

    public void Attack()
    {
        Debug.Log("Attacking Player Now!");
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, weaponRange, playerLayer);

        if (hits.Length > 0)
        {
            hits[0].GetComponent<PlayerHealth>().TakeDamage(damage);
        }
    }

    void Shoot()
    {
        Instantiate(SlimeSpit, attackPoint.position, Quaternion.identity);
    }
}
