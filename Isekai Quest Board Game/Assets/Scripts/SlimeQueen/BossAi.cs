using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossAi : MonoBehaviour
{
    public float currentHealth;
    public float maxHealth;
    public float damage = 5f;
    public bool phase2Activated = false;
    
    public Transform attackPoint;
    public VentPopping ventPopping;
    public PlayerHealth playerHealth;
    public GameObject soul;
    public GameObject SlimeSpit;
    public EnemyHealth health;

    private Rigidbody2D rb;
    private Animator anim;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        health = GetComponent<EnemyHealth>();
        currentHealth = health.bossCurrentHealth;
        maxHealth = health.bossMaxHealth;
    }

    // Update is called once per frame
    void Update(){

        CheckHealth();
    }

    void FixedUpdate(){

    }

    private void CheckHealth()
    {
        if (!phase2Activated && health.bossCurrentHealth <= health.bossMaxHealth / 2)
        {
            phase2Activated = true;
            Debug.Log("Phase 2 Activated");
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

    public IEnumerator DeathSequence()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Forest");

    }

    public void Shoot()
    {
        Instantiate(SlimeSpit, attackPoint.position, Quaternion.identity);
    }
}
