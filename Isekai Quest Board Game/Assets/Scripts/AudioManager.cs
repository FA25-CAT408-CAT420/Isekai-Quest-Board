using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Start is called before the first frame update
   [SerializeField] AudioSource musicSource;
   [SerializeField] AudioSource SFXSource;

   public AudioClip sword1; 
   public AudioClip sword2;
   public AudioClip potion;
   public AudioClip levelUp;
   public AudioClip selectionSFX; 
   public AudioClip positiveSFX;
   public AudioClip slimeHurt;
   public AudioClip playerHurt;
   public AudioClip nPCSpeak;
   public AudioClip videoGameTXT;

   public void PlaySFX(AudioClip clip)
   {
        SFXSource.PlayOneShot(clip);
   }
}
 

