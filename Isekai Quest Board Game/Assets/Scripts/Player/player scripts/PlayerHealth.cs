using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public GameManager gameManager;
    public PlayerMovement playerMovement;
    public float baseHP = 50f;
    public float currentHP;
    public float maxHP = 1000;
    public float MP = 50f;
    public float dmgAmount;
    public float dmgContainer;

    public bool isInvulnerable = false;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    void Start()
    {
        
    }
    void Update(){
        if (currentHP <= 0)
        {
            //Die();
        }
        if (currentHP > maxHP)
        {
            currentHP = maxHP;
        }
    }
    public void TakeDamage(float amount)
    {   
        if (isInvulnerable == false)
        {
            dmgContainer = amount;
            currentHP -= dmgAmount;
        }
        else if (isInvulnerable == true){
            Debug.Log("I AM IMMORTAL");
        }
    }

    private void Die()
    {
        //gameManager.gmStrengthSP = gameManager.gmHealthSP = gameManager.gmDefenseSP = 0;
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
