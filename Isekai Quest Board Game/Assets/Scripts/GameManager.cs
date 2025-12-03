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
    public bool soulDropped = false;
    public bool isDead = false;

    private PlayerInputActions inputActions;
    private InputAction submit;
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

    // Update is called once per frame
    void Update()
    {
        if (soulCounter != null)
        {
            soulCounter.text = soulPoints.ToString();
        }
        

        if (soulPoints < 0){
            soulPoints = 0;
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        submit = inputActions.UI.Submit;
        submit.performed += OnSubmit;
        
        inputActions.Enable();
        submit.Enable();
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        submit.performed -= OnSubmit;
        
        inputActions.Disable();
        submit.Disable();
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
            if(soulCounter != null)
            {
                soulCounter = GUI.transform.Find("SoulGroup/Soul Counter").GetComponent<TextMeshProUGUI>();
            }
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

    private void OnSubmit(InputAction.CallbackContext context)
    {
        if (isDead)
        {
            SceneManager.LoadScene("Forest");
        }
        
    }
    
}
