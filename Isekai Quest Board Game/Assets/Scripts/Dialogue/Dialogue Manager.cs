using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    AudioManager audioManager;
    public GameObject continueButton; 

    [Header("Dialogue stuff")]
    public Text nameText;
    public Text dialogueText;
    public float textSpeed;
    private bool isTyping = false;
    
    private Queue<string> sentences;

    // Start is called before the first frame update
    /*private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }*/

    void Start()
    {
        sentences = new Queue<string>();
        //DisplayNextSentence();
    }

    public void Update()
    {
        // if (Input.GetKeyDown("o"))
        // {
        //     DisplayNextSentence();
        // }
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

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (isTyping)
        {
            return;
        }

        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
        //dialogueText.text = sentence;
        //Debug.Log(sentence);
    }

    IEnumerator TypeSentence (string sentence)
    {
        isTyping = true;
        continueButton.SetActive(false);

        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }

        isTyping = false;
        continueButton.SetActive(true);
    }

    void EndDialogue()
    {
        Debug.Log("End of conversation. ");
    }

}
