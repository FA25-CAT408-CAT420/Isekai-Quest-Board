using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggroState : State
{

    public IdleState idle;
    public NavigateState navigate;

    public float attackRange = 1.5f;
    public VisionTrigger visionTrigger;
    public Transform target;

    private bool isChasing;

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
    }

    public override void Do()
    {

        if (target != null)
        {
            Chase();
        }

    }


    public override void Exit()
    {
        
    }

    void Chase()
    {
        if (target == null)
        {
            isComplete = true;
            return;
        }

        float distance = Vector2.Distance(core.transform.position, target.position);

        if (distance <= attackRange)
        {
            isChasing = false;
            Set(idle, true);
            rb.velocity = Vector2.zero;
            anim.SetBool("Moving", false);
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
