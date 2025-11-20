using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulsRetrieved : MonoBehaviour
{
    public int souls;
    public GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        souls = gameManager.soulPoints;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
