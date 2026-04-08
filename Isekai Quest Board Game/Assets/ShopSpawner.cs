using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopSpawner : MonoBehaviour
{
    private GameManager gameManager;
    public GameObject[] spellList;  // ← you can keep this if you need it locally

    void Awake()
    {

        
    }

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        if (gameManager == null)
        {
            Debug.LogError("ShopSpawner couldn't find GameManager!", this);
            return;
        }

        
    }

    void OnEnable()
    {
        gameManager = GameManager.Instance;

        if (gameManager != null)
        {
            gameManager.shopSpawners.Add(gameObject);
            Debug.Log("Spawner registered: " + name);
        }
        else
        {
            Debug.LogWarning("GameManager instance not found in OnEnable");
        }
    }

    void OnDisable()
    {
        if (gameManager != null)
        {
            gameManager.shopSpawners.Remove(gameObject);
        }
    }

    public void PopulateSpawners()
    {
        if (!gameManager.shopSpawners.Contains(gameObject))
        {
            gameManager.shopSpawners.Add(gameObject);
        }
    }
}