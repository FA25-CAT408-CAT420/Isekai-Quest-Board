using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatState : State
{
    public Transform target;
    public float attackCooldown = 50f;
    public float attackRange = 1.5f; 
    public float attackOne = 5f;
    public float attackTwo = 10f;

    //private bool isAttacking = false;
    //private float nextAttackTime = 0f;
    private float lastAttackTime;
    private string currentAttackName; 

    void RandomAttack()
    {
        int attackChoice = Random.Range(0,2); // 0 or 1

        if (attackChoice == 0)
        {
            currentAttackName = "attackOne";
            //DealDamage(attackOne);
            Debug.Log("Slime did 5 damage");
            lastAttackTime -= attackCooldown;
            
        } else
        {
            currentAttackName = "attackTwo";
            //DealDamage(attackTwo);
            Debug.Log("Slime did 10 damage");
            lastAttackTime -= attackCooldown;
        }

        lastAttackTime = Time.deltaTime;
    }

    void DealDamage(float amount)
    {
        // Check if target has health component
        PlayerHealth health = target.GetComponent<PlayerHealth>();
        if (health != null)
        {
            health.TakeDamage(amount);
        }
        else
        {
            Debug.LogWarning("Target has no PlayerHealth component!");
        }
    }

    public override void Enter()
    {
        lastAttackTime -= attackCooldown;

        RandomAttack();
        Debug.Log("Enemy Chose attack: " + currentAttackName);
        anim.SetBool("Attacking", true);
    }

    public override void Do()
    {
        if (target == null)
        {
            isComplete = true;
            return;
        }

        float distance = Vector2.Distance(core.transform.position, target.position);

        // Stop attacking if target is too far and go back to chase
        if (distance > attackRange)
        {
            isComplete = true;
            return;
        }

        /*if (!isAttacking && Time.deltaTime >= nextAttackTime)
        {
            StartCoroutine(PerformAttack());
        }*/

        // If cooldown passed, attack again
        if (Time.deltaTime >= lastAttackTime + attackCooldown)
        {
            RandomAttack();
        }
    }

    /*IEnumerator PerformAttack()
    {
        isAttacking = true;
        RandomAttack ();

    }*/

    public override void Exit() {
        anim.SetBool("Attacking", false);
    }
}
