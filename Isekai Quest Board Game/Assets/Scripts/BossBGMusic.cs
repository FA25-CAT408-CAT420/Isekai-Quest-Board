using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBGmusic : MonoBehaviour
{
    public static BossBGmusic instance;

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