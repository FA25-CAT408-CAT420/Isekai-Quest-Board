using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAi : EnemyCore
{
    public BossPatrol bossPatrol;
    public BossNavigation bossNavigation;

    // Start is called before the first frame update
    void Start()
    {
        SetUpInstances();

        bossPatrol.SetCore(this);
        bossNavigation.SetCore(this);

        bossPatrol.bossNavigation = bossNavigation;
        
        Set(bossPatrol);
        
    }

    // Update is called once per frame
    void Update()
    {

        state.DoBranch();
        
    }

    void FixedUpdate(){
          state.FixedDoBranch();
    }
}
