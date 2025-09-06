using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject GUI;
    private TextMeshProUGUI soulCounter;

    public int soulPoints = 0;

    void Awake(){
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        soulCounter.text = "TEMP SOUL COUNTER: " + soulPoints;
    }

    void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        GUI = GameObject.FindWithTag("UI");

        if (GUI == null) {
            Debug.LogWarning("No GUI in scene: " + scene.name);
        }
        else if (GUI != null) {
            soulCounter = GUI.transform.Find("Soul Counter").GetComponent<TextMeshProUGUI>();
        }
    }
}
