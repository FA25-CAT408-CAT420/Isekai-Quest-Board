using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public Direction openingDirection;
    // TOP --> need BOTTOM door
    // BOTTOM --> need TOP door
    // LEFT --> need RIGHT door
    // RIGHT --> need LEFT door

    private RoomTemplates templates;
    private int rand;
    private bool spawned = false;

    public Transform cameraBounds;
    public enum Direction
    {
        TOP, RIGHT, BOTTOM, LEFT
    }

    void Start(){
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        cameraBounds = GameObject.FindGameObjectWithTag("CameraBounds").transform;
        //Invoke calls a function with a time delay
        Invoke("Spawn", 0.1f);
    }

    void Spawn()
    {
        if (!spawned){
            if (openingDirection == Direction.TOP)
            {
                //Spawn a room with a BOTTOM door
                rand = Random.Range(0, templates.bottomRooms.Length);
                Instantiate(templates.bottomRooms[rand], transform.position, templates.bottomRooms[rand].transform.rotation, cameraBounds);
            }
            else if (openingDirection == Direction.BOTTOM)
            {
                //Spawn a room with a TOP door
                rand = Random.Range(0, templates.topRooms.Length);
                Instantiate(templates.topRooms[rand], transform.position, templates.topRooms[rand].transform.rotation, cameraBounds);
            }
            else if (openingDirection == Direction.LEFT)
            {
                //Spawn a room with a RIGHT door
                rand = Random.Range(0, templates.rightRooms.Length);
                Instantiate(templates.rightRooms[rand], transform.position, templates.rightRooms[rand].transform.rotation, cameraBounds);
            }
            else if (openingDirection == Direction.RIGHT)
            {
                //Spawn a room with a LEFT door
                rand = Random.Range(0, templates.leftRooms.Length);
                Instantiate(templates.leftRooms[rand], transform.position, templates.leftRooms[rand].transform.rotation, cameraBounds);
            }

            spawned = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other){
        if (other.CompareTag("SpawnPoint")){
            if (other.GetComponent<RoomSpawner>().spawned == false && spawned == false){
                Instantiate(templates.closedRoom, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
            spawned = true;
        }
    }
}
