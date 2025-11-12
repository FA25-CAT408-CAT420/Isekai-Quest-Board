using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAi : EnemyCore
{

    public BossMovement bossMovement;

    // Start is called before the first frame update
    void Start()
    {
        SetUpInstances();

        Set(bossMovement);
        
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
