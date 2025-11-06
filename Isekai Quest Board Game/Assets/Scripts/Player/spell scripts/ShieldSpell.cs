using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldSpell : Spells
{
    public GameObject shieldPrefab;
    public GameObject player;
    public float shieldTimer = 3f;
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerHealth>().isInvulnerable = true;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position;
    }

    void FixedUpdate()
    {
        shieldTimer -= Time.deltaTime;
        if (shieldTimer < 5f && shieldTimer > 2f)
        {
            animator.SetTrigger("BlinkSlow");
        }
        else if (shieldTimer < 2f)
        {
            animator.SetTrigger("BlinkFast");
        }

        if (shieldTimer <= 0)
        {
            player.GetComponent<PlayerHealth>().isInvulnerable = false;
            Destroy(gameObject);
        }
    }

    public override void Spell()
    {
        Instantiate(shieldPrefab);
        base.Spell();
    }

    public override void Cost()
    {
        baseMPCost = 10f;

    }

    public void HoldTrigger()
    {
        animator.SetTrigger("Hold");
    }
    // public void OnCollisionEnter2D(Collision2D other)
    // {
    //     if (other.gameObject.tag == "Enemy")
    //     {
    //         Debug.Log("ENEMY GOT HIT");
    //     }
    //     //Override Collision effects in proprietary spell
    // }
}
