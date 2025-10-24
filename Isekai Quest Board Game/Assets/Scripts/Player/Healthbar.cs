using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    public Slider healthSlider;
    public Slider easeHealthSlider;
    public Slider mpSlider;
    public Slider easeMPSlider;
    public float health;
    public float magicPoints;
    public float lerpSpeed = 0.05f;
    public PlayerHealth playerHealth;
    public PlayerHealth playerMP;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        health = playerHealth.HP;
        magicPoints = playerMP.MP;

        if (healthSlider.value != health)
        {
            healthSlider.value = health;
        }

        if (healthSlider.value != easeHealthSlider.value)
        {
            easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, health, lerpSpeed);
        }

        if (mpSlider.value != magicPoints)
        {
            mpSlider.value = magicPoints;
        }

        if (mpSlider.value != easeMPSlider.value)
        {
            easeMPSlider.value = Mathf.Lerp(easeMPSlider.value, magicPoints, lerpSpeed);
        }
    }

}
