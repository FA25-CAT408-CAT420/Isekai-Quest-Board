using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTraps : MonoBehaviour
{
    public SpikeDamage spkDMG;
    public string spikeID; // A, B, C, Etc.
    public Animator anim;

    
   public void ActivateSpike()
   {
        if (anim != null)
        {
            anim.SetTrigger("Activate");
        }
   }

    void EnableSpike()
    {
        spkDMG.isEnabled = true;
    }
    void DisableSpike()
    {

        spkDMG.isEnabled = false;
    }
}
