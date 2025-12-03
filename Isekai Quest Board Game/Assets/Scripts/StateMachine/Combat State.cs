using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatState : State
{
 public float attackDamageLight = 10f;
    public float attackDamageHeavy = 20f;

    public float attackCooldown = 1.5f;
    public float attackRange = 2f;

    public Transform target;

    private bool isAttacking = false;
    private float nextAttackTime = 0f;

    public override void Enter()
    {
        isAttacking = false;
        nextAttackTime = Time.time + 0.2f;
        anim.SetBool("Attacking", false);
    }

    public override void Do()
    {
        if (target == null)
        {
            isComplete = true;
            return;
        }

        float dist = Vector2.Distance(core.transform.position, target.position);

        if (dist > attackRange)
        {
            anim.SetBool("Attacking", false);
            isComplete = true;
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

        // Turn on the animator attack branch
        anim.SetBool("Attacking", true);

        // Wait 1 frame so animator updates and enters attack state
        yield return null;

        // Read the current attack animation
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        float animDuration = stateInfo.length;
        
        while (!stateInfo.IsTag("Attack"))
        {
            yield return null;
            stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        }

        animDuration = stateInfo.length;

        // === Deal damage (optional timing adjustment) ===
        ApplyAttackDamageBasedOnClip(stateInfo);

        // Wait for animation to complete
        yield return new WaitForSeconds(animDuration);

        // Turn off attacking so animator exits attack sub-state
        anim.SetBool("Attacking", false);

        nextAttackTime = Time.time + attackCooldown;
        isAttacking = false;
    }

    void ApplyAttackDamageBasedOnClip(AnimatorStateInfo info)
    {
        // OPTIONAL: use clip name to choose damage type
        string clip = info.shortNameHash.ToString();

        if (info.IsName("LightAttack"))
            DealDamage(attackDamageLight);
        else if (info.IsName("HeavyAttack"))
            DealDamage(attackDamageHeavy);
        else
            DealDamage(attackDamageLight); // fallback
    }

    void DealDamage(float dmg)
    {
        if (target == null) return;
        PlayerHealth hp = target.GetComponent<PlayerHealth>();
        if (hp != null)
            hp.TakeDamage(dmg);
    }
}
