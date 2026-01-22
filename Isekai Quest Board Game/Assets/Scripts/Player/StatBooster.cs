using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatBooster : MonoBehaviour
{
    float rateOfChange = 0.25f;
    GameManager gm = FindObjectOfType<GameManager>();
    public float BoostStats(float baseStat)
    {
        float e1 = (1 + rateOfChange);
        float e2 = gm.currentCycles;
        float e3 = Mathf.Pow(e1, e2);

        return baseStat *= e3;
    }
}
