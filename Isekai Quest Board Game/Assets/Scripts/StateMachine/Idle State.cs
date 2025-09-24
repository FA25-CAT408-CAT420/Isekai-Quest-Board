using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
   public override void Enter() {
    //animator.Play("Idle");
   }

   public override void Do() {
      isComplete = true;
   }

   public override void Exit(){

   }
}
