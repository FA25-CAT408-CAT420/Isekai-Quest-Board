using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyBase : MonoBehaviour
{
    [Header("Enemy States")]
    public float currentHealth;
    public float maxHealth;
    public float baseDamage = 5; 
    public int knockbackForce = 1;
    private int facingDirection = 1;
    public int AC = 10;
    public float speed;
    public float attackRange = 2;
    public float attackCooldown = 2;
    public float playerDetectedRange = 5;
    private float attackCooldownTimer;

    [Header("References")]
    public Transform detectionPoint;
    public LayerMask playerLayer;
    public PlayerHealth playerHealth;
    public GameObject soul;
    public EnemyHealth health;
    private Transform player;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;
    private EnemyState enemyState;

    [Header("Hitting Wall logic")]
    private int currentDirection;
    private float halfWidth;
    private float halfHeight;
    private Vector2 movement;

    [Header("Patrolling")]
    public float moveDuration = 0.5f;
    public float pauseDuration = 1f;
    private Coroutine patrolRoutine;
    private Coroutine chaseRoutine;
    private float gridSize = 1f;
    private bool isMoving = false;
    private Vector2 targetPosition;
    

    // Start is called before the first frame update

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        if (health != null)
        {
            health = GetComponent<EnemyHealth>();
            currentHealth = health.currentHealth;  
            maxHealth = health.maxHealth;
        }
    
        //StatCalc();
    }

    void Start()
    {
        halfWidth = sr.bounds.extents.x;
        halfHeight = sr.bounds.extents.y;
        currentDirection = facingDirection;
        sr.flipX = facingDirection == 1 ? false : true;
        ChangeState(EnemyState.Patrolling);
    }

    // Update is called once per frame
    void Update()
    {
        if (detectionPoint != null)
        {
            CheckForPLayer();
        }

        if (attackCooldownTimer >0)
        {
            attackCooldownTimer -= Time.deltaTime;
        }
        switch (enemyState)
        {
            case EnemyState.Patrolling:
                SetDirection();
                break;

            case EnemyState.Chasing:
                ChaseGrid();
                break;
    
            case EnemyState.Attacking:
                rb.velocity = Vector2.zero;
                isMoving = false;
                break;
        }
    }

    void ChaseGrid()
    {
        if (player == null) return;

        // If already moving towards a tile then continue moving
        if (isMoving)
        {
            ChasePlayer();
            return;
        }

        // Choose next tile step
        Vector2 direction = player.position - transform.position;

        Vector2 moveDir = Vector2.zero;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            moveDir = new Vector2(Mathf.Sign(direction.x), 0);
        }
        else {
            moveDir = new Vector2(0, Mathf.Sign(direction.y));
        }

        targetPosition = (Vector2)transform.position + moveDir;
        isMoving = true;
    }

    void ChasePlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetPosition) < 0.01f)
        {
            transform.position = targetPosition;
            isMoving = false;
        }
    }

    private void SetDirection()
    {
        Vector2 rightPos = transform.position;
        Vector2 leftPos = transform.position;
        rightPos.x += halfWidth;
        leftPos.x -= halfWidth;

        if (rb.velocity.x > 0)
        {
            if (Physics2D.Raycast(transform.position, Vector2.right, halfWidth + 0.1f, LayerMask.GetMask("Walls")))
            {
            // Draw a ray starting at the center of our enemy and point it to the right
            // Check to see if the raycast is intersecting with a wall
            // Also Check to make sure our enemy is actually WALKING right
            // if we don't do this check the enemy will get stuck moving constantly back and forth
            currentDirection *= -1;
            sr.flipX = true;
            }
            else if (Physics2D.Raycast(transform.position, Vector2.up, halfHeight + 0.1f, LayerMask.GetMask("Walls")))
            {
                currentDirection *= -1;
                sr.flipX = true;
            }

        }
        else if (rb.velocity.x < 0)
        {
            if (Physics2D.Raycast(transform.position, Vector2.left, halfWidth + 0.1f, LayerMask.GetMask("Walls")))
            {
            currentDirection *= -1;
            sr.flipX = false;
            }
            else if (Physics2D.Raycast(transform.position, Vector2.up, halfHeight + 0.1f, LayerMask.GetMask("Walls")))
            {
                currentDirection *= -1;
                sr.flipX = true;
            }

        }

        Debug.DrawRay(transform.position, Vector2.right * (halfWidth + 0.1f), Color.red);
        Debug.DrawRay(transform.position, Vector2.left * (halfWidth + 0.1f), Color.red);
        Debug.DrawRay(transform.position, Vector2.up * (halfHeight + 0.1f), Color.red);
        Debug.DrawRay(transform.position, Vector2.down * (halfHeight + 0.1f), Color.red);
    }


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
   }

   private IEnumerator PatrolRoutine() {
        while (enemyState == EnemyState.Patrolling)
        {

            Vector2 direction = Vector2.zero;

            // Randomly picks a direction for the slime to move towards.
            int randomDir = Random.Range(0,4);

            // A Switch case that assigns what each number will be used for
            switch (randomDir)
            {
                case 0: direction = Vector2.right; break;
                case 1: direction = Vector2.left; break;
                case 2: direction = Vector2.up; break;
                case 3: direction = Vector2.down; break;
            }

            // Flips the sprite when moving left and right
            if (direction.x > 0)
            {
                sr.flipX = false;
            }
            if (direction.x < 0)
            {
                sr.flipX = true;
            }
            
            // Calculates where the slime is and where the slime wants to go.
            Vector2 startPosition = rb.position;
            Vector2 targetPosition = startPosition + direction * gridSize;

            float elapsedTime = 0f;

            // How long it takes for the slime to move to that tile
            while (elapsedTime < moveDuration)
            {
                elapsedTime += Time.deltaTime;
                float percent = elapsedTime / moveDuration;

                Vector2 newPosition = Vector2.Lerp(startPosition, targetPosition, percent);
                rb.MovePosition(newPosition);

                yield return null;
            }

            // Makes sure the slime snaps exactly to the grid
            rb.MovePosition(targetPosition);

            // How long we want the slime to pause before moving again.
            yield return new WaitForSeconds(pauseDuration);
        }
   }

   void ChangeState(EnemyState newState)
   {
    //Exit the current animation
        if (enemyState == EnemyState.Idle)
        {
            anim.SetBool("isIdle", false);
        }
        else if (enemyState == EnemyState.Chasing){
            anim.SetBool("Moving", false);
        }
        else if (enemyState == EnemyState.Attacking){
            anim.SetBool("Attacking", false);
        }
        else if (enemyState == EnemyState.Patrolling){
            anim.SetBool("Moving", false);
        }
        
        if (patrolRoutine != null)
        {
            StopCoroutine(patrolRoutine);
            patrolRoutine = null;
        }

        rb.velocity = Vector2.zero;
        isMoving = false;

        //Update our current state
        enemyState = newState;

        //Update the new animation
        if (enemyState == EnemyState.Idle)
        {
            anim.SetBool("isIdle", true);
        }
        else if (enemyState == EnemyState.Chasing){
            anim.SetBool("Moving", true);
        }
        else if (enemyState == EnemyState.Attacking){
            anim.SetBool("Attacking", true);
        }
        else if (enemyState == EnemyState.Patrolling){
            anim.SetBool("Moving", true);

            if (patrolRoutine == null)
            {
                patrolRoutine = StartCoroutine(PatrolRoutine());
            }
        }
   }
   

//    private void OnDrawGizmosSelected()
//    {
//         Gizmos.color = Color.red;
//         Gizmos.DrawWireSphere(detectionPoint.position, playerDetectedRange);
//    }
}

public enum EnemyState
{
    Idle,
    Chasing,
    Attacking,
    Patrolling,
}
