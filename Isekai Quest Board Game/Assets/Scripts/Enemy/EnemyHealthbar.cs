using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthbar : MonoBehaviour
{
    public Slider healthSlider;
    public Slider easeHealthSlider;
    public float health;
    public float lerpSpeed = 0.05f;
    public EnemyBase enemyBase;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        health = enemyBase.currentHealth;
        
        if (healthSlider.value != health)
        {
            healthSlider.value = health;
        }
        if (healthSlider.value != easeHealthSlider.value)
        {
            easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, health, lerpSpeed);
        }
    }
}
