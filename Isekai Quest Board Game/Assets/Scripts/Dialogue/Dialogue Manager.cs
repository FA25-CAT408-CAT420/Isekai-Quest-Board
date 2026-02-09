using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    AudioManager audioManager; 

    public Text nameText;
    public Text dialogueText;
    
    private Queue<string> sentences;

    // Start is called before the first frame update
    /*private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }*/

    void Start()
    {
        sentences = new Queue<string>();
        DisplayNextSentence();
    }

    public void Update()
    {
        /*if (Input.GetKeyDown("o"))
        {
            DisplayNextSentence();
        }*/
    }


    public void StartDialogue (Dialogue dialogue)
    {
        //Debug.Log("Starting conversation with " + dialogue.name);

        nameText.text = dialogue.name;
        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            //audioManager.PlaySFX(audioManager.videoGameTXT);
            sentences.Enqueue(sentence);
        }

        //DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        dialogueText.text = sentence;
        //Debug.Log(sentence);
    }

    void EndDialogue()
    {
        Debug.Log("End of conversation. ");
    }

}
