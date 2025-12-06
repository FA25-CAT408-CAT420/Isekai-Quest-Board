using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class RoomTemplates : MonoBehaviour
{  
    public GameManager gameManager;
    public GameObject[] bottomRooms;
    public GameObject[] topRooms;
    public GameObject[] leftRooms;
    public GameObject[] rightRooms;

    public GameObject closedRoom;

    public List<GameObject> rooms;

    public float waitTime;
    private bool spawnedEnemy;
    private bool spawnedSoul;
    public GameObject[] levelEndSpawnables;
    public GameObject[] enemyType;
    public GameObject soul;
    int rand; 

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    void Update(){
        if(waitTime <= 0 && spawnedEnemy == false){
            for (int i = 0; i < rooms.Count; i++){
                if(i == rooms.Count - 1){
                    
                    if(gameManager.floorsCleared >= 3)
                    {
                        Instantiate(levelEndSpawnables[0], rooms[i].transform.position, Quaternion.identity);
                        spawnedEnemy = true;
                    }
                    else
                    {
                        Instantiate(levelEndSpawnables[1], rooms[i].transform.position, Quaternion.identity);
                        spawnedEnemy = true;
                    }

                    AstarPath.active.Scan();
                }
            }
        } else{
            waitTime -= Time.deltaTime;
        }

        if(waitTime <= 0 && spawnedSoul == false){
            for (int i = 0; i < rooms.Count; i++){
                rand = Random.Range(0, 4);
                EnemySpawner(i, rand);
                if(i == (rooms.Count - 1) / 2){
                    Instantiate(soul, rooms[i].transform.position, Quaternion.identity);
                    spawnedSoul = true;
                }
            }
        } else{
            waitTime -= Time.deltaTime;
        }
    }
    
    public void EnemySpawner(int i, int enemies)
    {
        for (int j = 0; j < enemies; j++)
        {
            int unlockedEnemiesCount = gameManager.bossesCleared;
            unlockedEnemiesCount = Mathf.Clamp(unlockedEnemiesCount, 1, enemyType.Length);
            int index = Random.Range(0, unlockedEnemiesCount);
            Instantiate(enemyType[index], rooms[i].transform.position, Quaternion.identity);
        }
    }
}
