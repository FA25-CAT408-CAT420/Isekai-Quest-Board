using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatManager : MonoBehaviour
{
    public PlayerCombat pc;
    public PlayerHealth ph;
    public GameManager gm;
    float rateOfChange = 0.25f;
    public float healthSP;
    public float strengthSP;
    public float defenseSP;

    void Awake()
    {
        gm = FindObjectOfType<GameManager>();
    }
    void Start()
    {
        CalculateStats();
        ph.currentHP = ph.maxHP;
    }

    public static float Round(float value, int digits)
    {
        float mult = Mathf.Pow(10.0f, (float)digits);
        return Mathf.Round(value * mult) / mult;
    }
        
    public float BoostStats(float baseStat, float upgradePoint)
    {
        float e1 = 1 + rateOfChange;
        float e2 = upgradePoint;
        float e3 = Mathf.Pow(e1, e2);
        baseStat *= e3;
        return Round(baseStat, 2);
    }

    public void CalculateStats()
    {
        healthSP = gm.gmHealthSP;
        strengthSP = gm.gmStrengthSP;
        defenseSP = gm.gmDefenseSP;
        pc.damage = BoostStats(pc.strength, strengthSP);
        ph.maxHP = BoostStats(ph.baseHP, healthSP);
    }
}
