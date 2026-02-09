using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
   public Dialogue dialogue;

   void Awake()
   {
      FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
   }


   /*public void FixedUpdate()
   {
      if (Input.GetKey("space"))
      {
         FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
      }
   }*/
   
   public void TriggerDialogue()
   {
        //FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
   }
}
