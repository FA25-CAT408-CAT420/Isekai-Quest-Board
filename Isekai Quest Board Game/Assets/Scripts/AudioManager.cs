using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Start is called before the first frame update
   public AudioSource SFXSource;
   public AudioClip TheClip; 

   public void PlaySFX()
   {
     SFXSource.PlayOneShot(TheClip);
   }
}
 

