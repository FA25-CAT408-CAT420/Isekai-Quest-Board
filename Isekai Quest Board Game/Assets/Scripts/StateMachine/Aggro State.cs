using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggroState : State
{

    public NavigateState navigate;

    public VisionTrigger visionTrigger;
    public Transform target;
    public float minimumDistance = 5f;

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
        if (target == null)
        {
            isComplete = true;
            return;
        }

        float distance = Vector2.Distance(core.transform.position, target.position);

        // If close enough switch to combat
        if (distance <= minimumDistance)
        {
            rb.velocity = Vector2.zero;
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
