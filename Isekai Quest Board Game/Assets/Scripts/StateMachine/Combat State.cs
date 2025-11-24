using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatState : State
{
    public float lightAttackDamage = 10f;
    public float heavyAttackDamage = 20f;

    public float extraDelayAfterAttack = 1.0f;   // delay AFTER animation finishes
    public float attackRange = 1.5f;

    public Transform target;  // assigned from AggroState

    private bool isAttacking = false;
    private float nextAttackTime = 0f;

    private string currentAttackName; 

    public override void Enter()
    {
        isAttacking = false;
        nextAttackTime = Time.time;

        // Start attacking immediately on entering combat
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

        // leave combat if player moves too far
        if (distance > attackRange)
        {
            isComplete = true;
            return;
        }

        if (isAttacking)
        {
            anim.SetBool("Attacking", true);
        }

        // If we are allowed to attack again and not currently in an attack animation
        if (!isAttacking && Time.time >= nextAttackTime)
        {
            StartCoroutine(PerformAttack());
        }
    }

    IEnumerator PerformAttack()
    {
        isAttacking = true;
        RandomAttack(); // Play animation + apply damage

        yield return null;

        // Get the currently playing animation's length
        AnimatorStateInfo animState = anim.GetCurrentAnimatorStateInfo(0);
        float animDuration = animState.length;

        // Wait for animation to finish + extra delay
        yield return new WaitForSeconds(animDuration + extraDelayAfterAttack);

        // Allow next attack
        nextAttackTime = Time.time;
        isAttacking = false;
    }

    void RandomAttack()
    {
        int choice = Random.Range(0, 2); // 0 or 1

        if (choice == 0)
        {
            currentAttackName = "LightAttack";
            DealDamage(lightAttackDamage);
        }
        else
        {
            currentAttackName = "HeavyAttack";
            DealDamage(heavyAttackDamage);
        }
    }

    void DealDamage(float amount)
    {
        PlayerHealth health = target.GetComponent<PlayerHealth>();

        if (health != null)
        {
            health.TakeDamage(amount);
        }
        else
        {
            Debug.LogWarning("CombatState: Target has no PlayerHealth component.");
        }
    }

    public override void Exit() {
    }
}
