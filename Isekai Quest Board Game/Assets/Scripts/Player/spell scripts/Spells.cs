using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spells : MonoBehaviour
{
    public PlayerHealth playerStat;
    public int spellLvl = 1;
    public float baseMPCost;
    public  Image sprite;

    void Awake(){
        GameObject playerOBJ = GameObject.FindGameObjectWithTag("Player");
        if (playerOBJ != null){
            playerStat = playerOBJ.GetComponent<PlayerHealth>();
        }
    }

    public virtual void Spell(){
        Debug.Log($"Activated spell: {GetType().Name}");
        //Override spell logic here
    }

    public virtual float CostCalculation(){
        return baseMPCost * (2f * (0.5f * spellLvl));
    }

    public virtual void Cost()
    {
        //Override Cost in proprietary spells
    }
    
}
