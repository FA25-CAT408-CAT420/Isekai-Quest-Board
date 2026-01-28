using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : EnemyBase
{
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {

        currentHealth += damage;

        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }
        else if (currentHealth <= 0)
        {
            Instantiate(soul, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
