using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAi : EnemyCore
{
    public PatrolState patrol;

    //public AggroState Aggro;

    void Start(){
        SetUpInstances();
        Set(patrol);

    }

    void Update(){
        if (state.isComplete) {
 /*           if (state == Aggro){
                Set(patrol);
            }
*/
        }
/*
        if (state == patrol) {
            Aggro.CheckForTarget();
            if (Aggro.target != null) {
                Set(Aggro);
            }
        }
*/
        state.DoBranch();
    }

    void FixedUpdate(){
          state.FixedDoBranch();
    }
}
