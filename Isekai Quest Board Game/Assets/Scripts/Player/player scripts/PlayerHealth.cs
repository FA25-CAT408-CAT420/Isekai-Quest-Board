using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public GameManager gameManager;
    public PlayerMovement playerMovement;
    public float HP = 50f;
    public float MP = 50f;

    public bool isInvulnerable = false;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    void Update(){
        if (HP <= 0)
        {
            Die();
        }
    }
    public void TakeDamage(float amount)
    {   
        if (isInvulnerable == false)
        {
            HP -= amount; 
        }
        else if (isInvulnerable == true){
            Debug.Log("I AM IMMORTAL");
        }
           
    }

    private void Die()
    {
        playerMovement.StopAllCoroutines();
        playerMovement.enabled = false;
        gameManager.soulDropped = true;
        gameManager.isDead = true;
        CanvasGroup dungeonUI = GameObject.FindGameObjectWithTag("UI").transform.Find("DungeonUI").GetComponent<CanvasGroup>();
        CanvasGroup deathUI = GameObject.FindGameObjectWithTag("UI").transform.Find("DeathUI").GetComponent<CanvasGroup>();
        dungeonUI.alpha = 0;
        StartCoroutine(FadeIn(deathUI, 2f));
        StartCoroutine(ZoomIn(deathUI.GetComponent<RectTransform>(), 2.5f, 1.2f));
    }

    public IEnumerator FadeIn(CanvasGroup canvasGroup, float duration)
    {
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = true;

        float time = 0;
        while (time < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 1f;
    }

    public IEnumerator ZoomIn(RectTransform rect, float duration, float targetScale)
    {
        Vector3 startScale = rect.localScale;
        Vector3 endScale = Vector3.one * targetScale;

        float time = 0;
        while (time < duration)
        {
            rect.localScale = Vector3.Lerp(startScale, endScale, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        rect.localScale = endScale;
    }


}
