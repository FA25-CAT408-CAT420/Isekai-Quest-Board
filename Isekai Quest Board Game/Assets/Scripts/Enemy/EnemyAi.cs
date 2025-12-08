using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAi : EnemyCore
{
    public PatrolState patrol;

    void Start()
    {
        SetUpInstances();
        Set(patrol);
    }

    void Update()
    {
        state.DoBranch();
    }

    void FixedUpdate()
    {
          state.FixedDoBranch();
    }
}
