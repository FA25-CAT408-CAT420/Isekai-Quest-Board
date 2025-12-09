using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other){
        if (other.gameObject.tag == "ClosedRoom"){
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "SpawnedRoom")
        {
            Destroy(other.gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D other){
        if (other.gameObject.tag == "SpawnedRoom")
        {
            Destroy(other.gameObject);
        }
    } 
}
