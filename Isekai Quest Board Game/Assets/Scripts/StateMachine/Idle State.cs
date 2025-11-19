using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    public NavigateState navigate;

   public override void Enter() {
    anim.SetFloat("X", navigate.destination.x);
    anim.SetFloat("Y", navigate.destination.y);
    }

   public override void Do() {
      isComplete = true;
   }

   public override void Exit(){

   }
}
