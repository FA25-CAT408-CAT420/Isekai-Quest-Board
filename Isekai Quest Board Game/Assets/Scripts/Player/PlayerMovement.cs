using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    
    private Rigidbody2D rb;
    public GameManager gm;
    public InputAction playerMovement;
    Vector2 moveDirection = Vector2.zero;
    public InputAction playerSpecial;
    Vector2 dpadDirection = Vector2.zero;

    [Header("Targeting")]
    public List<EnemyBase> yourEnemiesInRange = new List<EnemyBase>();
    int enemyIndex = 0;
    [SerializeField, HideInInspector]
    private EnemyBase targetedEnemy;

    [Header("Stats")]
    public float HP = 50f;
    public float speed = 5f;

    [Header("Leveling")]
    public int level = 1;
    public int spellSlotOneLvl = 1;
    public float dmgPerLvlMult = 1f;

    [Header("Attacking")]
    public float attackSpeed = 5f;
    public float attackTimer = 0f;
    public float dmgLowEnd = 1f;
    public float dmgHighEnd = 3;
    public int critDC = 20;
    public float critMultiplier = 1.5f;
    public int pBonus = 5;
    public int toHit;

    [Header("Special")]
    public List<SpecialAttacks> specials = new List<SpecialAttacks>();
    public SpecialAttacks specialBeta;

    void OnEnable()
    {
        playerMovement.Enable();
        playerSpecial.Enable();
    }

    void OnDisable()
    {
        playerMovement.Disable();
        playerSpecial.Disable();
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        specials.Add(specialBeta);
    }

    // Update is called once per frame
    void Update()
    {
        if (gm == null)
        {
            gm = FindObjectOfType<GameManager>();
            Debug.Log("Game Manager Assigned.");
        }
        if (HP > 0)
        {
            PlayerActions();
            PurgeEnemies();
            EnemyTargeting();
            PlayerStates();
            if (targetedEnemy != null)
            {
                SpecialInput();
            }
            else return;
        }
        else
        {

        }
    }
    private void FixedUpdate()
    {
        rb.velocity = moveDirection.normalized * speed;
    }
    public enum State
    {
        Freeroam,
        Battle,
        Flee,
    }

    void PlayerStates()
    {
        switch (state)
        {
            default:
            case State.Freeroam:
                //TBD
                //Debug.Log("State = FREEROAM");

                if (Input.GetKeyDown(KeyCode.JoystickButton0) && yourEnemiesInRange.Count > 0)
                {
                    Debug.Log("Battle Start");
                    state = State.Battle;
                }
                else if (yourEnemiesInRange.Count <= 0)
                {
                    state = State.Freeroam;
                }
                break;
            case State.Battle:
                Attack();
                Debug.Log("State = BATTLE");
                if (yourEnemiesInRange.Count <= 0)
                {
                    state = State.Freeroam;
                }
                break;
            case State.Flee:
                //time for enemies to un-aggro
                break;
        }
    }
    void PlayerActions()
    {
        moveDirection = playerMovement.ReadValue<Vector2>();
        dpadDirection = playerSpecial.ReadValue<Vector2>();
    }
    private State state;
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
                toHit = Random.Range(1, 20 + pBonus);
                Debug.Log("Hit = " + toHit);
                if (toHit >= targetedEnemy.AC)
                {
                    float dmgPerLvlMult = 1f;
                    int critRate = Random.Range(1, critDC + 1);
                    float attackDamage = Random.Range(dmgLowEnd, dmgHighEnd) * (dmgPerLvlMult += dmgPerLvlMult + (0.15f * level));
                    if (critRate == critDC)
                    {
                        attackDamage *= critMultiplier;
                        attackDamage = Mathf.Floor(attackDamage);
                    }
                    else
                    {
                        attackDamage = Mathf.Floor(attackDamage);
                    }

                    targetedEnemy.health -= attackDamage;
                    attackTimer = 0f;
                }
                else
                {
                    Debug.Log("MISS.");
                }
            }
        }
        else if (targetedEnemy == null)
        {
            attackTimer = 0f;
        }
    }

    public void SpecialInput()
    {
        float specialDMG = 0f;
        if (dpadDirection.y > 0)
        {
            Debug.Log("DPAD UP Pressed");
            specialDMG = specials[0].SpecialAttack();
        }
        else if (dpadDirection.x > 0)
        {
            Debug.Log("DPAD RIGHT Pressed");
            // specialDMG = specials[0].SpecialAttack();
        }
        else if (dpadDirection.y < 0)
        {
            Debug.Log("DPAD DOWN Pressed");
            // specialDMG = specials[0].SpecialAttack();
        }
        else if (dpadDirection.x < 0)
        {
            Debug.Log("DPAD LEFT Pressed");
            // specialDMG = specials[0].SpecialAttack();
        }
        targetedEnemy.health -= specialDMG;
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

        // if (other.gameObject.tag == "Special")
        // {
        //     EnemyBase eb = other.GetComponent<EnemyBase>();
        //     AddYourEnemyToList(eb);
        // }
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
