using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInteractions : MonoBehaviour
{
    public GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void OnTriggerEnter2D (Collider2D other){
        if (other.gameObject.CompareTag("Soul"))
        {
            gameManager.soulPoints++;
            Destroy(other.gameObject);
        }
    }
}
