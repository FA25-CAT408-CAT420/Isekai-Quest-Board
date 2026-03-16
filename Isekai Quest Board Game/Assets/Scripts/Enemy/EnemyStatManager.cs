using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatManager : MonoBehaviour
{
    public GameManager gm;
    public EnemyCombat ec;
    public EnemyHealth eh;
    float rateOfChange = 0.25f;

    void Awake()
    {
        gm = FindObjectOfType<GameManager>();
        CalculateStats();
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

    void CalculateStats()
    {
        eh.maxHealth = BoostStats(eh.baseHealth, gm.worldLevel);
        ec.outDamage = BoostStats(ec.baseDamage, gm.worldLevel);
    }
}
