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
    public int floorsCleared = 0;
    public int bossesCleared = 0;

    public List<GameObject> totalSpells = new List<GameObject>();
    public List<GameObject> spellsToSpawn = new List<GameObject>();
    public List<GameObject> shopSpawners = new List<GameObject>();
    private bool spellsPopulated = false;

    [Header("Transition/Spawn")]
    public string nextSpawnID = ""; // ID of spawn point in next scene

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
            return;
        }

        inputActions = new PlayerInputActions();
    }

    void Update()
    {
        if (soulCounter != null)
        {
            soulCounter.text = soulPoints.ToString();
        }

        if (soulPoints < 0) soulPoints = 0;


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

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Handle UI
        GUI = GameObject.FindWithTag("UI");
        if (GUI != null)
        {
            soulCounter = GUI.transform.Find("SoulGroup/Soul Counter")?.GetComponent<TextMeshProUGUI>();
        }

        // Spawn player at the correct spawn point
        SpawnPlayerAtNextSpawn();

        if (scene.name == "DeathScene")
        {
            inputActions.UI.Restart.performed += Restart;
        }
        else
        {
            inputActions.UI.Restart.performed -= Restart;
        }

        if (scene.name == "Forest")
        {
            StartCoroutine(SpawnSpells());
        }
        else
        {
            spellsPopulated = false;
        }
    }

    IEnumerator SpawnSpells()
    {   
        yield return null;
        if (!spellsPopulated)
            {
                for (int i = 0; i < totalSpells.Count; i++)
                {
                    spellsToSpawn.Add(totalSpells[i]);
                }

                spellsPopulated = true; 
            }   

        if (spellsPopulated)
        {
            for (int j = 0; j < shopSpawners.Count; j++)
            {
                int r = Random.Range(0, spellsToSpawn.Count); 
                GameObject newSpell = Instantiate(spellsToSpawn[r], shopSpawners[j].transform.position, Quaternion.identity);
                SpellAcquisition spellScript = newSpell.GetComponent<SpellAcquisition>();
                spellScript.prefabReference = spellsToSpawn[r];
                spellsToSpawn.RemoveAt(r);
            }
        }
    }

    private void SpawnPlayerAtNextSpawn()
    {
        if (string.IsNullOrEmpty(nextSpawnID)) return;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        SpawnPoint[] points = FindObjectsOfType<SpawnPoint>();
        foreach (var p in points)
        {
            if (p.spawnID == nextSpawnID)
            {
                player.transform.position = p.transform.position;
                player.transform.rotation = Quaternion.identity;
                Debug.Log("Spawned player at: " + nextSpawnID);
                return;
            }
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
