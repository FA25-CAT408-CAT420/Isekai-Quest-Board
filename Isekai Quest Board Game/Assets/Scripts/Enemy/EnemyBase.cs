using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyBase : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth;


    public SpriteRenderer sr;
    public Material normalMat;
    public Material outlineMat;
    public bool isTargeted = false;
    public int AC = 10;
    public TextMeshProUGUI displayHealth;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        sr = GetComponent<SpriteRenderer>();
        
    }

    // Update is called once per frame
    void Update()
    {
        DisplayHealth();
        TargetOutline();
    }

    public void ChangeHealth(int amount)
    {
        currentHealth += amount;

        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }
        else if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void DisplayHealth()
    {
        displayHealth.text = "Health:" + currentHealth;
    }

    public void TargetOutline()
    {
        if (isTargeted)
        {
            sr.material = outlineMat;
        }
        else if (!isTargeted)
        {
            sr.material = normalMat;
        }
    }
}
