using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyBase : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth;
    public int knockbackForce = 1;
    private int facingDirection = 1;

    // public SpriteRenderer sr;
    // public Material normalMat;
    // public Material outlineMat;
    public bool isTargeted = false;
    public int AC = 10;
    public float speed;
    public float attackRange = 2;
    public float attackCooldown = 2;
    public float playerDetectedRange = 5;
    public Transform detectionPoint;
    public LayerMask playerLayer;

    public PlayerHealth playerHealth;
    public GameObject soul;

    private float attackCooldownTimer;
    private Transform player;
    private Rigidbody2D rb;
    private Animator anim;
    private EnemyState enemyState;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;
        ChangeState(EnemyState.Idle);
        // sr = GetComponent<SpriteRenderer>();
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckForPLayer();

        if (attackCooldownTimer >0)
        {
            attackCooldownTimer -= Time.deltaTime;
        }
        if (enemyState == EnemyState.Chasing)
        {
            Chase();
        }
        else if(enemyState == EnemyState.Attacking)
        {
            //Attacky stuff
            rb.velocity = Vector2.zero;
        }
        // TargetOutline();
    }

    void Chase()
    {
        
        if (player.position.x > transform.position.x && facingDirection == -1 ||
                player.position.x < transform.position.x && facingDirection == 1)
        {
            Flip();
        }

            Vector2 direction = (player.position - transform.position).normalized;
            rb.velocity = direction * speed;
    }

    void Flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
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
    }

    // public void TargetOutline()
    // {
    //     if (isTargeted)
    //     {
    //         sr.material = outlineMat;
    //     }
    //     else if (!isTargeted)
    //     {
    //         sr.material = normalMat;
    //     }
    // }

    private void CheckForPLayer() {
        Collider2D[] hits = Physics2D.OverlapCircleAll(detectionPoint.position, playerDetectedRange, playerLayer);
        if (hits.Length > 0)
        {
            player = hits[0].transform;

            //if the player is in attack range AND cooldown is ready
            if (Vector2.Distance(transform.position, player.position) <= attackRange && attackCooldownTimer <= 0)
            {
                attackCooldownTimer = attackCooldown;
                ChangeState(EnemyState.Attacking);
            }

            else if (Vector2.Distance(transform.position, player.position) > attackRange && enemyState != EnemyState.Attacking)
            {
                ChangeState(EnemyState.Chasing);
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
            ChangeState(EnemyState.Idle);
        }
   }

   void ChangeState(EnemyState newState)
   {
    //Exit the current animation
        if (enemyState == EnemyState.Idle)
        {
            //anim.SetFloat("X", rb.position.x);
            //anim.SetFloat("Y", rb.position.y);
            anim.SetBool("isIdle", false);
        }
        else if (enemyState == EnemyState.Chasing){
            anim.SetBool("Moving", false);
        }
        else if (enemyState == EnemyState.Attacking){
            anim.SetBool("Attacking", false);
        }

        //Update our current state
        enemyState = newState;

        //Update the new animation
        if (enemyState == EnemyState.Idle)
        {
            anim.SetFloat("X", rb.position.x);
            anim.SetFloat("Y", rb.position.y);
        }
        else if (enemyState == EnemyState.Chasing){
            anim.SetBool("Moving", true);
        }
        else if (enemyState == EnemyState.Attacking){
            anim.SetBool("Attacking", true);
        }
   }

   private void OnDrawGizmosSelected()
   {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(detectionPoint.position, playerDetectedRange);
   }
}

public enum EnemyState
{
    Idle,
    Chasing,
    Attacking,
}
