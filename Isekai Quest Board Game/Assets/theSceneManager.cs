using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class theSceneManager : MonoBehaviour
{
    public GameManager gameManager;
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    public void TitleScreen()
    {
        SceneManager.LoadScene("Title Screen");
    }
    public void CreditScene()
    {
        SceneManager.LoadScene("Credits");
    }
    public void SceneStart()
    {
        SceneManager.LoadScene("Forest");
    }
    public void LoadCutScene()
    {
        gameManager.newGame = true;
        SceneManager.LoadScene("PROLOGUE");
    }
}
