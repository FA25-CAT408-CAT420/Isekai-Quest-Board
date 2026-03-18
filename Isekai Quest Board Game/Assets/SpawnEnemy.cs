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
    public float sTimer;
    public float sMaxTime = 5;

    public float sTimeDelay;
    public bool isSpawner = false;
    public EnemySpawnManager sm;
    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    void Start()
    {
        sTimer = sTimeDelay;
        unlockedEnemiesCount = gameManager.bossesCleared;
        index = Random.Range(0, unlockedEnemiesCount);
        unlockedEnemiesCount = Mathf.Clamp(unlockedEnemiesCount, 1, enemyType.Length);
        spawn = Random.Range(0,2);
        if (spawn > 0)
        {
            Spawn();
        }
        else
        {
            Debug.Log("No Enemies Spawned");
        }
    }

    void Update()
    {
        sTimer += Time.deltaTime;
        if (sm != null)
        {
            if (sm.spawnedEnemies >= sm.maxCapacity)
            {
                return;
            }
            else if (sm.spawnedEnemies < sm.maxCapacity)
            {
                if (isSpawner && sTimer >= sMaxTime)
                {
                    sTimer = sTimeDelay;
                    Spawn();
                }
            }
        }
        else
        {
            return;
        }


    }

    void Spawn()
    {
        Instantiate(enemyType[index], this.transform.position, Quaternion.identity);
    }
}
