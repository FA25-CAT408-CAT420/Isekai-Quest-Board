using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition : MonoBehaviour
{
    public string sceneToLoad;
    public string nextSpawnID; // the ID of the spawn point in the next scene
    public bool useTrigger = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (useTrigger && collision.CompareTag("Player"))
        {
            TransitionScene();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!useTrigger && collision.collider.CompareTag("Player"))
        {
            TransitionScene();
        }
    }

    private void TransitionScene()
    {
        // Save spawn ID before loading next scene
        if (GameManager.Instance != null)
        {
            GameManager.Instance.nextSpawnID = nextSpawnID;
        }

        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogWarning("Transition object has no scene assigned!");
        }
    }
}