using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition : MonoBehaviour
{
    [Header("Scene to Load")]
    [Tooltip("Name of the scene to load when the player touches this object.")]
    public string sceneToLoad;

    [Header("Optional Settings")]
    public bool useTrigger = true; // If true, uses OnTriggerEnter2D; if false, uses OnCollisionEnter2D

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
