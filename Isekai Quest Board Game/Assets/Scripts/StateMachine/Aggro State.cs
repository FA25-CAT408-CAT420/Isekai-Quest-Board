using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggroState : State
{

    public CombatState combat;
    public NavigateState navigate;

    //public VisionTrigger visionTrigger;
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
    }

    public override void Do()
    {
        if (target == null)
        {
            isComplete = true;
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

    /*public Transform CheckForTarget()
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
    }*/
}
