using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.InputSystem;
using Cinemachine;

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
    public int worldLevel = 1;
    public bool isWorldLvlRaised;

    public List<GameObject> totalSpells = new List<GameObject>();
    public List<GameObject> spellsToSpawn = new List<GameObject>();
    public List<GameObject> shopSpawners = new List<GameObject>();
    public List<Spells> specials = new List<Spells>();
    private bool spellsPopulated = false;
    public CinemachineVirtualCamera camera;

    [Header("Transition/Spawn")]
    public string nextSpawnID = ""; // ID of spawn point in next scene

    [Header("Shop Spawn Delay")]
    [Tooltip("Seconds to wait after Forest loads before spawning shop items (helps with registration timing)")]
    public float spawnDelaySeconds = 0.8f;  // ← Start here, try 1.0–1.5 if still 0

    private float spawnTimer = 0f;
    private bool waitingToSpawn = false;

    private PlayerInputActions inputActions;
    private InputAction submit;

    [Header("Skill Points Saved")]
    public float gmHealthSP;
    public float gmStrengthSP;
    public float gmDefenseSP;

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

        // Timer runs here — starts immediately on scene load
        if (waitingToSpawn)
        {
            spawnTimer += Time.deltaTime;

            if (spawnTimer >= spawnDelaySeconds)
            {
                waitingToSpawn = false;
                StartCoroutine(SpawnSpells());
                Debug.Log($"Spawn delay complete ({spawnTimer:F2}s) — starting SpawnSpells (spawners: {shopSpawners.Count})");
            }
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

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Handle UI
        GUI = GameObject.FindWithTag("UI");
        if (GUI != null)
        {
            soulCounter = GUI.transform.Find("DungeonUI/SoulGroup/Soul Counter")?.GetComponent<TextMeshProUGUI>();
        }

        if (camera == null)
        {
            camera = GameObject.FindGameObjectWithTag("Camera").GetComponent<CinemachineVirtualCamera>();
        }
        else if (camera != null)
        {
            Debug.Log("Camera successfully loaded");
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
            isWorldLvlRaised = false;
            spellsPopulated = false;
            spellsToSpawn.Clear();

            // Start timer immediately on load
            spawnTimer = 0f;
            waitingToSpawn = true;

            Debug.Log($"Forest loaded — spawn timer started ({spawnDelaySeconds}s delay)");
        }
        else
        {
            spellsPopulated = false;
            waitingToSpawn = false;
        }
    }

    IEnumerator SpawnSpells()
    {  
        // Optional: one extra frame for safety
        yield return null;

        Debug.Log($"SpawnSpells activated — spawners found: {shopSpawners.Count}");

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
            if (shopSpawners.Count == 0)
            {
                Debug.LogWarning("No shop spawners registered — check ShopSpawner scripts or increase delay!");
            }

            for (int j = 0; j < shopSpawners.Count && spellsToSpawn.Count > 0; j++)
            {
                int r = Random.Range(0, spellsToSpawn.Count);

                GameObject newItem = Instantiate(
                    spellsToSpawn[r],
                    shopSpawners[j].transform.position,
                    Quaternion.identity
                );

                IShopInterface shopItem = newItem.GetComponent<IShopInterface>();
                if (shopItem != null)
                {
                    shopItem.Initialize(spellsToSpawn[r]);
                }
                else
                {
                    Debug.LogError($"Shop item missing IShopInterface: {newItem.name}");
                }

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

    public void SceneStart()
    {
        SceneManager.LoadScene("Forest");
    }

    public void creditScene()
    {
        SceneManager.LoadScene("Credits");
    }

    public void titleScreen()
    {
        SceneManager.LoadScene("Title Screen");
    }
}