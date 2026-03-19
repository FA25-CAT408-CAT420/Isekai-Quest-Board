using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonBGMusic : MonoBehaviour
{
    public static DungeonBGMusic instance;

    void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
}