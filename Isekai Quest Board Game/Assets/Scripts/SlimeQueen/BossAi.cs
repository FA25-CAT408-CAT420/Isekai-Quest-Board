using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAi : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth;
    public float damage = 5f;
    public bool phase2Activated = false;
    
    public Transform attackPoint;
    public VentPopping ventPopping;
    public PlayerHealth playerHealth;
    public GameObject soul;
    public GameObject SlimeSpit;

    private Rigidbody2D rb;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update(){
        
    }

    void FixedUpdate(){

    }

    public void ChangeHealth(int amount)
    {
        currentHealth += amount;

        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }
        else if (currentHealth <= 0)
        {
            Instantiate(soul, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        //Phase 2 check
        if (!phase2Activated && currentHealth <= maxHealth / 2)
        {
            phase2Activated = true;
            ventPopping.ActivatePhase2();
        }
    }

    void OnCollisionEnter2D (Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);
            other.gameObject.GetComponent<PlayerMovement>().StopMovementCoroutine();
            other.gameObject.GetComponent<PlayerKnockback>().ApplyKnockback(transform.position);
        }
    }

    public void Shoot()
    {
        Instantiate(SlimeSpit, attackPoint.position, Quaternion.identity);
    }
}
