using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandSpell : Spells
{
    public GameObject sandPrefab;
    public GameObject player;
    private Vector2 launchDirection;
    public Rigidbody2D rb;
    float speed = 5f;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = player.transform.position;
        rb = GetComponent<Rigidbody2D>();

        if (launchDirection == Vector2.zero)
        {
            launchDirection = Vector2.right;
        }

        rb.velocity = launchDirection.normalized * speed;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void FixedUpdate()
    {
    }

    public override void Spell()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        launchDirection = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().pos1;
        GameObject sandObj = Instantiate(sandPrefab, player.transform.position, Quaternion.identity);
        sandObj.GetComponent<SandSpell>().Initialize(launchDirection);
        base.Spell();


    }
    
    public void Initialize(Vector2 dir)
    {
        launchDirection = dir;
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
            other.gameObject.GetComponent<EnemyBase>().speed = 0.5f;
        }
    }
}
