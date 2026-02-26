using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearRoom : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameManager gameManager;
    public GameObject enemyPrefab;

    [Tooltip("Minimum enemies to spawn (at least 1 recommended)")]
    public int minEnemies = 3;

    [Tooltip("Maximum enemies to spawn")]
    public int maxEnemies = 6;

    [Tooltip("How far from room edges enemies should spawn (prevents clipping walls)")]
    public float spawnPadding = 0.6f;

    [Header("Clear & Scene Settings")]
    [Tooltip("Exact name of the scene to load when room is cleared")]
    public string nextSceneName = "Forest";

    [Tooltip("Tag that all enemies have")]
    public string enemyTag = "Enemy";


    private List<GameObject> spawnedEnemies = new List<GameObject>();
    private Collider2D roomTrigger;
    private bool hasSpawned = false;


    void Start()
    {
        roomTrigger = GetComponent<Collider2D>();
        gameManager = FindObjectOfType<GameManager>();
        if (roomTrigger == null || !roomTrigger.isTrigger)
        {
            Debug.LogError($"{nameof(ClearRoom)} needs a trigger Collider2D on this GameObject!");
            return;
        }

        if (enemyPrefab == null)
        {
            Debug.LogError("Enemy prefab is not assigned!");
            return;
        }

        SpawnEnemies();
    }


    private void SpawnEnemies()
    {
        if (hasSpawned) return;
        hasSpawned = true;

        int count = Random.Range(minEnemies, maxEnemies + 1);
        Debug.Log($"Room spawning {count} enemies...");

        Bounds bounds = roomTrigger.bounds;

        for (int i = 0; i < count; i++)
        {
            // Random position inside bounds with padding
            float x = Random.Range(bounds.min.x + spawnPadding, bounds.max.x - spawnPadding);
            float y = Random.Range(bounds.min.y + spawnPadding, bounds.max.y - spawnPadding);

            Vector3 pos = new Vector3(x, y, transform.position.z);

            // Extra safety: only spawn if point is actually inside the trigger
            if (roomTrigger.OverlapPoint(pos))
            {
                GameObject enemy = Instantiate(enemyPrefab, pos, Quaternion.identity);
                spawnedEnemies.Add(enemy);
            }
            else
            {
                // Rare — retry once
                i--;
            }
        }

        if (spawnedEnemies.Count == 0)
        {
            Debug.LogWarning("No enemies were spawned — room will clear instantly.");
        }
    }


    void Update()
    {
        // Remove any enemies that were destroyed
        spawnedEnemies.RemoveAll(e => e == null);

        // Only load next scene after we've spawned enemies and now they're all gone
        if (hasSpawned && spawnedEnemies.Count == 0)
        {
            LoadNextScene();
        }
    }


    private void LoadNextScene()
    {
        gameManager.floorsCleared++;
        if (gameManager.floorsCleared >= 3){
            nextSceneName = "Boss Room";
        }
        else if (gameManager.floorsCleared < 3)
        {
            nextSceneName = "Dungeon Depths";
        }
        if (string.IsNullOrEmpty(nextSceneName))
        {
            Debug.LogWarning("nextSceneName is empty — cannot load scene.");
            return;
        }

        Debug.Log($"Room cleared ({spawnedEnemies.Count} enemies remaining). Loading {nextSceneName}");
        SceneManager.LoadScene(nextSceneName);

        // Prevent multiple loads
        enabled = false;
    }


    // Optional: keep track if enemies somehow leave the room
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(enemyTag))
        {
            spawnedEnemies.Remove(other.gameObject);
        }
    }


    // Visual feedback in editor
    void OnDrawGizmosSelected()
    {
        if (roomTrigger != null)
        {
            Gizmos.color = spawnedEnemies.Count > 0 ? new Color(1f, 0.3f, 0.3f, 0.5f) : new Color(0.3f, 1f, 0.3f, 0.5f);
            Gizmos.DrawCube(roomTrigger.bounds.center, roomTrigger.bounds.size);
        }
    }
}