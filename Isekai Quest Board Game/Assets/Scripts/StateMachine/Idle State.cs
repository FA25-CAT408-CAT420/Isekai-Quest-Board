using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    public NavigateState navigate;

   public override void Enter() {
    anim.SetFloat("X", rb.position.x);
    anim.SetFloat("Y", rb.position.y);
    }

   public override void Do() {
      isComplete = true;
   }

   public override void Exit(){

   }
}
