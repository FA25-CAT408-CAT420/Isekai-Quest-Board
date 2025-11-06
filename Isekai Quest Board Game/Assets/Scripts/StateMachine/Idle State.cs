using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
   public override void Enter() {
    anim.SetBool("isWalking", false);
   }

   public override void Do() {
      isComplete = true;
   }

   public override void Exit(){

   }
}
