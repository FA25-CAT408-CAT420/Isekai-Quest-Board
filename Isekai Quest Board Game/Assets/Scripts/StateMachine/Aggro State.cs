using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggroState : State
{
     public float attackRange = 1.5f;
    public float attackCooldown = 1.5f;

    public VisionTrigger visionTrigger;
    public Transform target;

    public NavigateState navigate;

    bool isAttacking = false;
    float nextAttackTime = 0f;

    public override void Enter()
    {
        if (target == null)
        {
            isComplete = true;
            return;
        }

        anim.SetBool("Moving", true);
        anim.SetBool("Attacking", false);

        // Start chasing immediately
        navigate.destination = target.position;
        Set(navigate, true);
    }

    public override void Do()
    {
        if (target == null)
        {
            isComplete = true;
            return;
        }

        float dist = Vector2.Distance(core.transform.position, target.position);

        // --- OUT OF RANGE → CHASE ---
        if (dist > attackRange)
        {
            anim.SetBool("Attacking", false);

            if (machine.state != navigate)
                Set(navigate, true);

            navigate.destination = target.position;
            anim.SetBool("Moving", true);
            return;
        }

        if (dist <= attackRange)
        {
            // stop movement visuals
            anim.SetBool("Moving", false);

            // ensure the rigidbody isn't moving while attacking
            if (rb != null)
                rb.velocity = Vector2.zero;

            // only start an attack if not currently attacking and cooldown passed
            if (!isAttacking && Time.time >= nextAttackTime)
            {
                StartCoroutine(AttackSequence());
            }
        }
    }

    IEnumerator AttackSequence()
    {
        isAttacking = true;

        anim.SetBool("Attacking", true);

        // Wait 1 frame for animator to update
        yield return null;

        // Read the current attack animation
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);

        // Ensure we are really inside the attack substate
        while (!stateInfo.IsTag("Attack"))
        {
            yield return null;
            stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        }

        float animLength = stateInfo.length;

        // DAMAGE: optional timing, keep simple for now
        DealDamageToPlayer();

        // Wait for the full attack animation to finish
        yield return new WaitForSeconds(animLength);

        // Turn off attacking
        anim.SetBool("Attacking", false);

        // Set cooldown
        nextAttackTime = Time.time + attackCooldown;
        isAttacking = false;
    }

    void DealDamageToPlayer()
    {
        if (target == null) return;
        PlayerHealth hp = target.GetComponent<PlayerHealth>();
        if (hp != null)
        {
            hp.TakeDamage(10); // or a variable
        }
    }

    public override void Exit()
    {
        anim.SetBool("Moving", false);
        anim.SetBool("Attacking", false);
    }

    public Transform CheckForTarget()
    {
        if (visionTrigger != null && visionTrigger.playerDetected)
        {
            target = visionTrigger.detectedTarget; 
            return target;
        } 
        else 
        {
            target = null;
            isComplete = true; 
            return null;
        }
    }
}
