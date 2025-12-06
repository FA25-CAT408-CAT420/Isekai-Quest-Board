using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAi : EnemyCore
{
    public PatrolState patrol;

    public AggroState aggro;

    public float damage = 5;

    void Start(){
        SetUpInstances();
        Set(patrol);

    }

    void Update()
    {

    /*if (state == patrol) 
    {
        aggro.CheckForTarget();
        if (aggro.target != null)
         {
            Set(aggro);
         }
    }*/
        state.DoBranch();
}

    void FixedUpdate(){
          state.FixedDoBranch();
    }
}
