using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public float health = 5f;
    public SpriteRenderer sr;
    public Material normalMat;
    public Material outlineMat;
    public bool isTargeted = false;
    public int AC = 10;
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Health();
        TargetOutline();
    }

    public void Health()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
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
