using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestBGmusic : MonoBehaviour
{
    public static ForestBGmusic instance;

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