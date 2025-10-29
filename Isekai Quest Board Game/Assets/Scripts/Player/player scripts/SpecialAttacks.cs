using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSpell", menuName = "Spell/Spell")]
public class SpecialAttacks : ScriptableObject
{

    public int spellLevel = 1;
    public int diceAmount = 1;
    public int diceType = 4;
    public int spellCritDC = 20;
    public float spellCritMult = 1.5f;
    public float Spell()
    {
        int specCritRate = Random.Range(1, spellCritDC + 1);
        float spellMult = 2f * (0.5f * spellLevel);
        float specAtkDamage = (diceAmount * spellMult) * diceType;
        if (specCritRate >= spellCritDC)
        {
            specAtkDamage *= spellCritMult;
            specAtkDamage = Mathf.Floor(specAtkDamage);
        }
        else
        {
            specAtkDamage = Mathf.Floor(specAtkDamage);
        }

        return specAtkDamage;

        //targetedEnemy.health -= specAtkDamage;
        //state = State.Battle;
    }
}
