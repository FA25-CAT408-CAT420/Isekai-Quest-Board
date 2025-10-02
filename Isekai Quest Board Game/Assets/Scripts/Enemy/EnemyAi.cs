using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAi : EnemyCore
{
    public PatrolState patrol;

    public AggroState aggro;

    void Start(){
        SetUpInstances();
        Set(patrol);

    }

    void Update(){
        if (state.isComplete) {
            if (state == aggro) {
                Set(patrol);
        }
    }
    if (state == patrol) {
        aggro.CheckForTarget();
        if (aggro.target != null) {
            Set(patrol);
        } else {
            Set(patrol);
        }
    }
        state.DoBranch();
}

    void FixedUpdate(){
          state.FixedDoBranch();
    }
}
