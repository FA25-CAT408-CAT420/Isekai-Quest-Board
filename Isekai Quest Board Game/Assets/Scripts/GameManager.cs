using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject GUI;
    private TextMeshProUGUI soulCounter;

    public int soulPoints = 0;

    private PlayerInputActions inputActions;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        inputActions = new PlayerInputActions();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        soulCounter.text = soulPoints.ToString();

    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        inputActions.Enable();
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        inputActions.Disable();
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GUI = GameObject.FindWithTag("UI");

        if (GUI == null)
        {
            Debug.LogWarning("No GUI in scene: " + scene.name);
        }
        else if (GUI != null)
        {
            soulCounter = GUI.transform.Find("SoulGroup/Soul Counter").GetComponent<TextMeshProUGUI>();

        }

        if (scene.name == "DeathScene")
        {
            inputActions.UI.Restart.performed += Restart;
        }
        else
        {
            inputActions.UI.Restart.performed -= Restart;
        }
    }
    
    private void Restart(InputAction.CallbackContext context)
    {
        SceneManager.LoadScene("Testing Chambers");
    }
}
