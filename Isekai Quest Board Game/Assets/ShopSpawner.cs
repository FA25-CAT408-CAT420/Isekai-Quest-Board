using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopSpawner : MonoBehaviour
{
    private GameManager gameManager;
    public GameObject[] spellList;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        PopulateSpawners();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // public void PopulateSpells()
    // {
    //     spellList = new GameObject[gameManager.totalSpells.Length];
    //     for (int i = 0; i < gameManager.totalSpells.Length; i++)
    //     {
    //         spellList[i] = gameManager.totalSpells[i]; 
    //     }
    // }

    public void PopulateSpawners()
    {
        gameManager.shopSpawners.Add(gameObject);
    }
}
