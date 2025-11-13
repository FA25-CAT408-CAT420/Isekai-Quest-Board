using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLockedRoom : MonoBehaviour
{
    public GameObject lockedRoom;

    public void Spawn(){
        Instantiate(lockedRoom);
        Destroy(gameObject);
    }
}
