using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggroState : State
{
    public CombatState combat;
    public NavigateState navigate;
    public IdleState idle;
    public float minimumDistance;
    public Transform target;
    public float vision = 1f;

    public override void Enter() {
        navigate.destination = target.position; 
        Set(navigate, true);
    
    }

    public override void Do() {

        if (machine.state == navigate) {
            if (CloseEnough(target.position)) {
                Set(combat, true);
                rb.velocity = new Vector2(0,0);
            } else {
                navigate.destination = target.position;
                Set(navigate, true);
            }
        } else {
            if (target == null) {
            isComplete = true;
        }
        }
    }

    public override void Exit() {

    }

    public bool CloseEnough(Vector2 targetPos){
        return Vector2.Distance(core.transform.position, targetPos) < minimumDistance;
    }
    public Transform CheckForTarget(){
        if (Vector2.Distance(core.transform.position, target.position) < vision){
        return target;
    } else {
    return null;
    }
    }
}
