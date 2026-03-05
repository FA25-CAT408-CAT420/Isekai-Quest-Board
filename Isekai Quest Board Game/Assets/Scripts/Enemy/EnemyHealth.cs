using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float currentHealth;
    public float maxHealth;
    public GameObject soul;
    public BossAi slimeQueen;

    public float bossCurrentHealth;
    public float bossMaxHealth;

    AudioManager audioManager;
    // Start is called before the first frame update
    public void Awake()
    {
        //audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    public void Start()
    {
        currentHealth = maxHealth;
        bossCurrentHealth = bossMaxHealth;
    }

    public void TakeDamage(float damage)
    {

        currentHealth += damage;
        bossCurrentHealth += damage;

        //audioManager.PlaySFX(audioManager.slimeHurt);

        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }
        else if (currentHealth <= 0)
        {
            Instantiate(soul, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        if (bossCurrentHealth >= bossMaxHealth)
        {
            bossCurrentHealth = bossMaxHealth;
        }
        else if (bossCurrentHealth <= 0)
        {
            Instantiate(soul, transform.position, Quaternion.identity);
            slimeQueen.StartCoroutine(slimeQueen.DeathSequence());
        }
    }
}
