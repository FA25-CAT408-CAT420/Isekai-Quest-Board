using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAi : EnemyCore
{
    public VentPopping ventPopping;

    // Start is called before the first frame update
    void Start()
    {
        SetUpInstances();

        Set(ventPopping);
        
    }

    // Update is called once per frame
    void Update(){
        state.DoBranch();
        
    }

    void FixedUpdate(){
          state.FixedDoBranch();
    }
}
