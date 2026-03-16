using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{  
    public float baseHealth;
    public float currentHealth;
    public float maxHealth;
    public GameObject soul;
    public BossAi slimeQueen;
    public bool isBoss = false;

    AudioManager audioManager;
    // Start is called before the first frame update
    public void Awake()
    {
        //audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    public void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {

        currentHealth += damage;

        //audioManager.PlaySFX(audioManager.slimeHurt);

        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }
        else if (currentHealth <= 0)
        {
            Instantiate(soul, transform.position, Quaternion.identity);
            
            if (isBoss && slimeQueen != null)
            {
                slimeQueen.StartCoroutine(slimeQueen.DeathSequence());
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
