using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody2D rb;
    public GameManager gm;

    [Header("Targeting")]
    public List<EnemyBase> yourEnemiesInRange = new List<EnemyBase>();
    int enemyIndex = 0;
    [SerializeField, HideInInspector]
    private EnemyBase targetedEnemy;
    

    [Header("Attacking")]
    public float attackSpeed = 5f;
    public float attackTimer = 0f;
    public float dmgMultiplier = 1f;
    public float dmgLowEnd = 1f;
    public float dmgHighEnd = 3;
    public int critDC = 20;
    public float critMultiplier = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        rb.velocity = new Vector3(moveHorizontal, moveVertical).normalized * speed;

        if (gm == null)
        {
            gm = FindObjectOfType<GameManager>();
            Debug.Log("Game Manager Assigned.");
        }
        PurgeEnemies();
        EnemyTargeting();
        Attack();

    }
    public void EnemyTargeting()
    {
        if (yourEnemiesInRange.Count <= 0)
        {
            if (targetedEnemy != null)
            {
                targetedEnemy.isTargeted = false;
                targetedEnemy = null;
            }
            enemyIndex = 0;
            return;
        }
        
        else
        {
            enemyIndex = Mathf.Clamp(enemyIndex, 0, yourEnemiesInRange.Count - 1);
            targetedEnemy = yourEnemiesInRange[enemyIndex];

            if (Input.GetKeyDown(KeyCode.RightBracket))
            {
                enemyIndex = (enemyIndex + 1) % yourEnemiesInRange.Count;
            }
            else if (Input.GetKeyDown(KeyCode.LeftBracket))
            {
                enemyIndex--;
                if (enemyIndex < 0)
                {
                    enemyIndex = yourEnemiesInRange.Count - 1;
                }
            }

            for (int i = 0; i < yourEnemiesInRange.Count; i++)
            {
                yourEnemiesInRange[i].isTargeted = i == enemyIndex;
            }
        } 
    }

    public void AddYourEnemyToList(EnemyBase enemy)
    {
        if (!yourEnemiesInRange.Contains(enemy))
        {
            yourEnemiesInRange.Add(enemy);
            Debug.Log("Enemy added: " + enemy.name);
        }
    }

    public void RemoveYourEnemyFromList(EnemyBase enemy)
    {
        if (yourEnemiesInRange.Contains(enemy))
        {
            enemy.isTargeted = false;
            int deletedIndex = yourEnemiesInRange.IndexOf(enemy);
            yourEnemiesInRange.Remove(enemy);
            Debug.Log("Enemy removed: " + enemy.name);

            if (yourEnemiesInRange.Count == 0)
            {
                enemyIndex = 0;
                targetedEnemy = null;
            }
            else if (deletedIndex <= enemyIndex)
            {
                // Move target back by 1 if the removed enemy was before or at current index
                enemyIndex = Mathf.Clamp(enemyIndex - 1, 0, yourEnemiesInRange.Count - 1);
                targetedEnemy = yourEnemiesInRange[enemyIndex];
            }
        }
    }

    public void PurgeEnemies()
    {
        yourEnemiesInRange.RemoveAll(item => item == null);
        enemyIndex = Mathf.Clamp(enemyIndex, 0, yourEnemiesInRange.Count - 1);
    }

    public void Attack()
    {
        if (targetedEnemy != null)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackSpeed)
            {
                int critRate = Random.Range(1, critDC + 1);
                float attackDamage = Random.Range(dmgLowEnd, dmgHighEnd) * dmgMultiplier;
                if (critRate == critDC)
                {
                    attackDamage *= critMultiplier;
                }

                targetedEnemy.health -= attackDamage;
                attackTimer = 0f;
            }
        }
        else if (targetedEnemy == null)
        {
            attackTimer = 0f;
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Soul")
        {
            gm.soulPoints++;
            Destroy(other.gameObject);
        }

        if (other.gameObject.tag == "Enemy")
        {
            EnemyBase eb = other.GetComponent<EnemyBase>();
            AddYourEnemyToList(eb);
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            EnemyBase eb = other.GetComponent<EnemyBase>();
            RemoveYourEnemyFromList(eb);
        }
    }
}
