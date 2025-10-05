using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggroState : State
{
    public CombatState combat;
    public NavigateState navigate;
    public IdleState idle;

    public float minimumDistance;
    public VisionTrigger visionTrigger;
    public Transform target;

    public override void Enter() {
        if (target == null)
        {
            isComplete = true;
            return;
        }

        navigate.destination = target.position; 
        Set(navigate, true);
    
    }

    public override void Do() {

        if (machine.state == navigate) 
        {
            if (CloseEnough(target.position)) 
            {
                Set(combat, true);
                rb.velocity = new Vector2(0,0);
            } else {
                navigate.destination = target.position;
                Set(navigate, true);
            }
        }
    }


    public override void Exit() {

    }

    public bool CloseEnough(Vector2 targetPos){
        return Vector2.Distance(core.transform.position, targetPos) < minimumDistance;
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
