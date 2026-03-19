using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleBGMusic : MonoBehaviour
{
    // Start is called before the first frame update

    public static TitleBGMusic instance;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
            
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        if (SceneManager.GetActiveScene().name == "Level 3")
            BGmusic.instance.GetComponent<AudioSource>().Pause();
    }
}