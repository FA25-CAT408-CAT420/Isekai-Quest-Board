using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxMan : MonoBehaviour
{
    public GameObject aIcon;


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
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            aIcon.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
