using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionTrigger : MonoBehaviour
{ 
    private bool isChasing;

   void OnTriggerEnter2D(Collider2D collision) {
    if (collision.gameObject.tag == "Player")
    {
        isChasing = true;
    }
   }

   void OnTriggerExit2D(Collider2D collision) {
    if (collision.gameObject.tag == "Player")
    {
        isChasing = false;
    }
   }
}
