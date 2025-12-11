using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatState : State
{
    public AggroState aggro;
    //public GameObject slimeSpit;

    public override void Enter()
    {
        
    }

    public override void Do()
    {
        anim.SetTrigger("IsAttacking");
    }

    public override void Exit()
    {

    }
}
