using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopSpawner : MonoBehaviour
{
    private GameManager gameManager;
    public GameObject[] spellList;  // ← you can keep this if you need it locally

    void Awake()
    {
        Debug.Log($"[ShopSpawner {gameObject.name}] Awake at time: {Time.time:F2}");
        gameManager = FindObjectOfType<GameManager>();

        if (gameManager == null)
        {
            Debug.LogError("ShopSpawner couldn't find GameManager!", this);
            return;
        }

        PopulateSpawners();
    }

    void Start()
    {
        // If you still need something in Start, keep it here
        // But registration should be in Awake now
    }

    public void PopulateSpawners()
    {
        if (!gameManager.shopSpawners.Contains(gameObject))
        {
            gameManager.shopSpawners.Add(gameObject);
            Debug.Log($"[ShopSpawner] Registered {gameObject.name} in Awake()");
        }
    }
}