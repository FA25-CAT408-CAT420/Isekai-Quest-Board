using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatManager : MonoBehaviour
{
    PlayerCombat pc;
    PlayerHealth ph;
    float rateOfChange = 0.25f;
    public float healthSP = 0;
    public float strengthSP = 0;
    public float defenseSP = 0;
    void Awake()
    {
        pc = GetComponent<PlayerCombat>();
        ph = GetComponent<PlayerHealth>();
    }
    public float BoostStats(float baseStat, float upgradePoint)
    {
        float e1 = 1 + rateOfChange;
        float e2 = upgradePoint;
        float e3 = Mathf.Pow(e1, e2);

        return baseStat *= e3;
    }


}
