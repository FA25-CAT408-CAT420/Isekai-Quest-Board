using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatState : State
{
    public Transform target;
    public float attackRange = 1.5f; 
    public float attackOne = 0.5f;
    public float attackTwo = 0.5f;
    public float extraDelayAfterAttack = 2.5f;

    private bool isAttacking = false;
    private float nextAttackTime = 0f;
    private string currentAttackName; 

    void RandomAttack()
    {
        int attackChoice = Random.Range(0,2); // 0 or 1

        if (attackChoice == 0)
        {
            currentAttackName = "attackOne";
            anim.SetBool("Attacking", true);
            DealDamage(attackOne);
            Debug.Log("Slime did 5 damage");
            
        } else
        {
            currentAttackName = "attackTwo";
            anim.SetBool("Attacking", true);
            DealDamage(attackTwo);
            Debug.Log("Slime did 10 damage");
        }
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
        isAttacking = false;
        nextAttackTime = Time.time;
        StartCoroutine(PerformAttack());
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
            anim.SetBool("Attacking", false);
            return;
        }

        if (!isAttacking && Time.time >= nextAttackTime)
        {
            StartCoroutine(PerformAttack());
        }

    }
        IEnumerator PerformAttack()
        {
            isAttacking = true;
            RandomAttack();

            AnimatorStateInfo animState = anim.GetCurrentAnimatorStateInfo(0);
            float animDuration = animState.length;

            yield return new WaitForSeconds(animDuration + extraDelayAfterAttack);

            nextAttackTime = Time.time;
            isAttacking = false;
        }

    public override void Exit() {
    }
}
