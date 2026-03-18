using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject[] enemyType;
    int spawn;
    int index;
    int unlockedEnemiesCount;
    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    void Start()
    {
        unlockedEnemiesCount = gameManager.bossesCleared;
        index = Random.Range(0, unlockedEnemiesCount);
        unlockedEnemiesCount = Mathf.Clamp(unlockedEnemiesCount, 1, enemyType.Length);
        spawn = Random.Range(0,1);
        if (spawn > 0)
        {
            Spawn();
        }
        else
        {
            Debug.Log("No Enemies Spawned");
        }
    }

    void Spawn()
    {
        Instantiate(enemyType[index], transform.position, Quaternion.identity);
    }
}
