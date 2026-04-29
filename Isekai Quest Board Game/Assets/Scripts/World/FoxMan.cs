using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxMan : MonoBehaviour
{
    public GameObject aIcon;
    public FoxWizard wizard;
    public bool canInteract;


    // Start is called before the first frame update
    private void Start()
    {
        aIcon.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            aIcon.SetActive(true);
            canInteract = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            aIcon.SetActive(false);
            canInteract = false;
        }
    }

    public void Update()
    {
        if(wizard.isTalking)
        {
            canInteract = false;
        }
    }
}
