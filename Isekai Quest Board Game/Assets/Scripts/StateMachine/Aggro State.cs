using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggroState : State
{

    public CombatState combat;
    public NavigateState navigate;

    public float attackRange = 2.5f;
    public VisionTrigger visionTrigger;
    public Transform target;

    public override void Enter()
    {

        if (target == null)
        {
            isComplete = true;
            return;
        }

        navigate.destination = target.position;
        Set(navigate, true);

        anim.SetBool("Moving", true);
        anim.SetBool("Attacking", false);
    }

    public override void Do()
    {
        if (target == null)
        {
            isComplete = true;
            return;
        }

        float distance = Vector2.Distance(core.transform.position, target.position);

        if (distance <= attackRange)
        {
            rb.velocity = Vector2.zero;
            anim.SetBool("Moving", false);
            anim.SetBool("Attacking", true);

            Set(combat, true);
            return;
        }

        if (machine.state == navigate)
        {
            navigate.destination = target.position;
        }
        else
        {
            navigate.destination = target.position;
            Set(navigate, true);
        }
    }


    public override void Exit()
    {
        anim.SetBool("Moving", false);
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
