using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoxWizard : MonoBehaviour
{

    public GameObject dialogueBox;
    public DialogueTrigger dialogueTrigger; 
    public DialogueManager dialogueManager;


    // Start is called before the first frame update
    void Start()
    {
        dialogueBox.SetActive(false); 
    }

    public void Interacted()
    {
        //Debug.Log("TALKIN YO");
        StartCoroutine(HandleDialogue());
    }

    private IEnumerator HandleDialogue()
    {
        //Show Dialogue box
        dialogueBox.SetActive(true);

        //Start Dialogue
        dialogueManager.StartDialogue(dialogueTrigger.dialogue);

        //Wait unitl dialogue finishes
        yield return new WaitUntil(() => !dialogueManager.IsDialogueActive);
        dialogueBox.SetActive(false);
    }

    
}
