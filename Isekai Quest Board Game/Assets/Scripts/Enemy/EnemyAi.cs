using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAi : EnemyCore
{
    public PatrolState patrol;

    public AggroState aggro;

    void Start(){
        SetUpInstances();
        Set(aggro);

    }

    void Update(){

        state.DoBranch();
    }

    void FixedUpdate(){
          state.FixedDoBranch();
    }
}
