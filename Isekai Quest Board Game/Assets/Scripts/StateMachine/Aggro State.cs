using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggroState : State
{
    public CombatState combat;
    public NavigateState navigate;

    public float minimumDistance;
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
    
    }

    public override void Do()
    {
        if (machine.state == navigate) 
        {
            float distance = Vector2.Distance(core.transform.position, target.position);

            if (distance <= minimumDistance) 
            {
                Set(combat, true);
                rb.velocity = new Vector2(0,0);
                return;
            } else 
            {
                navigate.destination = target.position;
                Set(navigate, true);
            }
        }
    }


    public override void Exit()
    {
        
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
