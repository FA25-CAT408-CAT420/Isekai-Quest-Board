using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    public int spawnedEnemies = 0;
    public int maxCapacity = 3;

    void Update()
    {
        if (spawnedEnemies < 0)
        {
            spawnedEnemies = 0;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            spawnedEnemies++;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            spawnedEnemies--;
        }
    }
    
}
