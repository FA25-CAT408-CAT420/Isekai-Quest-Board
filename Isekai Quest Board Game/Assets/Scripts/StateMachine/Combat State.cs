using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatState : State
{
    public float lightAttackDamage = 10f;
    public float heavyAttackDamage = 20f;

    public float attackCooldown = 1.5f;
    public float attackRange = 1.5f;

    public Transform target;  // assigned from AggroState

    private bool isAttacking = false;
    private float nextAttackTime = 0f;

    private string currentAttackName; 

    public override void Enter()
    {
        isAttacking = false;
        nextAttackTime = Time.time + 0.2f;  // small buffer

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

        // If we are allowed to attack again and not currently in an attack animation
        if (!isAttacking && Time.time >= nextAttackTime)
        {
            StartCoroutine(PerformAttack());
            anim.SetBool("Attacking", true);
        }
    }

    IEnumerator PerformAttack()
    {
        isAttacking = true;

        // Randomly choose attack
        string attackTrigger = (Random.Range(0, 2) == 0) ? "LightAttack" : "HeavyAttack";
        float damage = (attackTrigger == "LightAttack") ? lightAttackDamage : heavyAttackDamage;

        // Wait 1 frame so animator updates
        yield return null;

        // Now safely read animation length
        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
        float duration = state.length;

        // Deal damage immediately (or delay if you want hit timing)
        DealDamage(damage);

        // Wait for animation + cooldown
        yield return new WaitForSeconds(duration);

        nextAttackTime = Time.time + attackCooldown;
        isAttacking = false;
    }

    void DealDamage(float amount)
    {
        PlayerHealth hp = target.GetComponent<PlayerHealth>();
        if (hp != null)
            hp.TakeDamage(amount);
    }

    public override void Exit() {
    }
}
