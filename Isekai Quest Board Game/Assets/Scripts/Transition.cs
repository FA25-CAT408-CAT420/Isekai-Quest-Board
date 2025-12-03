using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition : MonoBehaviour
{
    private GameManager gameManager;
    public string sceneToLoad;
    public bool useTrigger = true;
    private void Start()
    {
        // Find the GameManager in the scene
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (useTrigger && collision.CompareTag("Player"))
        {
            LoadScene();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!useTrigger && collision.collider.CompareTag("Player"))
        {
            LoadScene();
        }
    }

    private void LoadScene()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogWarning($"Transition object '{gameObject.name}' has no scene assigned.");
        }
    }
}
