using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionTrigger : MonoBehaviour
{

    public bool playerDetected = false; 
    public Transform detectedTarget; 

   void OnTriggerEnter2D(Collider2D other) {
    if (other.CompareTag("Player"))
    {
        playerDetected = true;
        detectedTarget = other.transform;
    }
   }

   void OnTriggerExit2D(Collider2D other) {
    if (other.CompareTag("Player"))
    {
        playerDetected = false;
        detectedTarget = null;
    }
   }
}
