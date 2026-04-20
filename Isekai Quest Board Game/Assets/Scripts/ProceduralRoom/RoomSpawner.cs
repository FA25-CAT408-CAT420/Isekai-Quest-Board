using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public Direction openingDirection;

    // TOP    --> need BOTTOM door
    // BOTTOM --> need TOP door
    // LEFT   --> need RIGHT door
    // RIGHT  --> need LEFT door

    private RoomTemplates templates;
    private int rand;
    private bool spawned = false;

    public Transform cameraBounds;   // Optional: for organizing rooms in hierarchy
    public float waitTime = 4f;

    public enum Direction
    {
        TOP, RIGHT, BOTTOM, LEFT
    }

    void Start()
    {
        Destroy(gameObject, waitTime);

        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();

        // Only find cameraBounds if you actually want to parent the rooms
        if (cameraBounds == null)
            cameraBounds = GameObject.FindGameObjectWithTag("CameraBounds")?.transform;

        Invoke("Spawn", 0.1f);
    }

    void Spawn()
    {
        if (spawned) return;

        GameObject newRoom = null;

        switch (openingDirection)
        {
            case Direction.TOP:
                // Need a room with a BOTTOM door
                rand = Random.Range(0, templates.bottomRooms.Length);
                newRoom = Instantiate(templates.bottomRooms[rand], transform.position, 
                                      templates.bottomRooms[rand].transform.rotation);
                break;

            case Direction.BOTTOM:
                // Need a room with a TOP door
                rand = Random.Range(0, templates.topRooms.Length);
                newRoom = Instantiate(templates.topRooms[rand], transform.position, 
                                      templates.topRooms[rand].transform.rotation);
                break;

            case Direction.LEFT:
                // Need a room with a RIGHT door
                rand = Random.Range(0, templates.rightRooms.Length);
                newRoom = Instantiate(templates.rightRooms[rand], transform.position, 
                                      templates.rightRooms[rand].transform.rotation);
                break;

            case Direction.RIGHT:
                // Need a room with a LEFT door
                rand = Random.Range(0, templates.leftRooms.Length);
                newRoom = Instantiate(templates.leftRooms[rand], transform.position, 
                                      templates.leftRooms[rand].transform.rotation);
                break;
        }

        // Optional: Parent the room under cameraBounds while keeping world position
        if (newRoom != null && cameraBounds != null)
        {
            newRoom.transform.SetParent(cameraBounds, worldPositionStays: true);
        }

        spawned = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("SpawnPoint"))
        {
            RoomSpawner otherSpawner = other.GetComponent<RoomSpawner>();

            if (otherSpawner != null && !otherSpawner.spawned && !spawned)
            {
                Instantiate(templates.closedRoom, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }

            spawned = true;
        }
    }
}