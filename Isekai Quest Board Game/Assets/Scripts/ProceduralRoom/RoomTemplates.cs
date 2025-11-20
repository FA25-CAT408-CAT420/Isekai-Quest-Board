using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public GameObject enemy;
    public GameObject soul;

    void Update(){
        if(waitTime <= 0 && spawnedEnemy == false){
            for (int i = 0; i < rooms.Count; i++){
                if(i == rooms.Count - 1){
                    Instantiate(enemy, rooms[i].transform.position, Quaternion.identity);
                    spawnedEnemy = true;
                }
            }
        } else{
            waitTime -= Time.deltaTime;
        }

        if(waitTime <= 0 && spawnedSoul == false){
            for (int i = 0; i < rooms.Count; i++){
                if(i == (rooms.Count - 1) / 2){
                    Instantiate(enemy, rooms[i].transform.position, Quaternion.identity);
                    spawnedSoul = true;
                }
            }
        } else{
            waitTime -= Time.deltaTime;
        }
    }
}
