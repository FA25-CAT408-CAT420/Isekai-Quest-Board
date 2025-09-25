using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggroState : State
{

    public NavigateState navigate;

    public IdleState idle;

    //public AttackState Attack;

    public Transform target;

    public float minimumDistance;

    public float vision; 

    public override void Enter() {
        navigate.destination = target.position;
        Set(navigate, true);

    }

    public override void Do() {

        if (state == navigate) {
            if (InRange(target.position)) {
                Set(idle, true);
                target.gameObject.SetActive(false);
                return;
                
            } else if (!InVision(target.position)) {
                Set(idle, true);
            } else {
                navigate.destination = target.position;
                Set(navigate, true);
            }
        }else {
            if (state.time > 1) {
                isComplete = true;
            }
        }

        if (target == null) {
            isComplete = true;
            return;
        }
    }

    public override void Exit() {

    }

    public bool InRange(Vector2 targetPos) {
        return Vector2.Distance(core.transform.position, targetPos) < minimumDistance; 
    }

    public bool InVision(Vector2 targetPos) {
        return Vector2.Distance(core.transform.position, targetPos) < vision;
    }

    public Transform CheckForTarget() {
        if (InVision(target.position)) {
            return target;
        }

        return null;
    }
}
