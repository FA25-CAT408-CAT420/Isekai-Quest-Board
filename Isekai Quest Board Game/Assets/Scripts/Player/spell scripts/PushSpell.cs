using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushSpell : Spells
{
    public GameObject pushPrefab;
    public GameObject player;
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        
        transform.position = player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void FixedUpdate()
    {
        Debug.Log("I LIVE");
    }

    public override void Spell()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Instantiate(pushPrefab, player.transform.position, Quaternion.identity);
        base.Spell();


    }

    public override void Cost()
    {
        baseMPCost = 10f;

    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Debug.Log("ENEMY GOT HIT");
            other.gameObject.GetComponent<EnemyKnockback>().Knockback(gameObject.transform, 15);
        }
    }
}
