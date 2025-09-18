using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyBase : MonoBehaviour
{
    public float health = 5f;
    public SpriteRenderer sr;
    public Material normalMat;
    public Material outlineMat;
    public bool isTargeted = false;
    public int AC = 10;
    public TextMeshProUGUI displayHealth;
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Health();
        DisplayHealth();
        TargetOutline();
    }

    public void Health()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void DisplayHealth()
    {
        displayHealth.text = "Health:" + health;
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
