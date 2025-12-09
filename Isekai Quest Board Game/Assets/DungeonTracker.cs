using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonTracker : MonoBehaviour
{   
    private GameManager gameManager;
    public bool useTrigger = true;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (useTrigger && collision.CompareTag("Player"))
        {
            gameManager.floorsCleared++;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!useTrigger && collision.collider.CompareTag("Player"))
        {
            gameManager.floorsCleared++;
        }
    }
}
