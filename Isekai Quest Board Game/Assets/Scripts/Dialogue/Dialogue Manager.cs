using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    AudioManager audioManager;
    public GameObject continueButton;
    public DialogueTrigger dialogueTrigger; 

    [Header("Dialogue stuff")]
    public Text nameText;
    public Text dialogueText;
    public float textSpeed;
    public bool isTyping = false;
    public bool enableSkip = true;
    public bool IsDialogueActive {get; private set;}
    
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
        if (Input.GetKeyDown("p"))
        {
            SkipDialogue();
        }
    }


    public void StartDialogue (Dialogue dialogue)
    {
        //Debug.Log("Starting conversation with " + dialogue.name);
        IsDialogueActive = true;

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

    async void EndDialogue()
    {
        Debug.Log("End of conversation. ");

        if(dialogueTrigger.cutsceneDialogue)
        {
            await ScreenFader.Instance.FadeOut();

            SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            CloseDialogue();
        }
    }

    public void CloseDialogue()
    {
        IsDialogueActive = false;
    }

    public void SkipDialogue()
    {
        if (!enableSkip) return;

        StopAllCoroutines();
        sentences.Clear();
        EndDialogue();
    }

}
