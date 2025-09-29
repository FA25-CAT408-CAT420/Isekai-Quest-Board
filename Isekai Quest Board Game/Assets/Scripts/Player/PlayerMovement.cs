using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance;
    [Header("Inputs")]
    private Rigidbody2D rb;
    public GameManager gm;
    public PlayerInputActions playerControls;
    Vector2 moveDirection = Vector2.zero;
    Vector2 dpadDirection = Vector2.zero;
    private InputAction interact;
    private InputAction move;
    private InputAction specialUp;
    private InputAction specialDown;
    private InputAction specialLeft;
    private InputAction specialRight;
    private InputAction targetNext;
    private InputAction targetPrev;

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
    public bool isTargeting = false;

    [Header("Special")]
    public List<SpecialAttacks> specials = new List<SpecialAttacks>();
    public SpecialAttacks specialBeta;

    private void Awake()
    {
        playerControls = new PlayerInputActions();

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void OnEnable()
    {
        move = playerControls.Player.Move;
        move.Enable();

        interact = playerControls.Player.Interact;
        interact.Enable();
        interact.performed += Interact;

        specialUp = playerControls.Player.SpecialUp;
        specialDown = playerControls.Player.SpecialDown;
        specialLeft = playerControls.Player.SpecialLeft;
        specialRight = playerControls.Player.SpecialRight;
        targetNext = playerControls.Player.TargetNext;
        targetPrev = playerControls.Player.TargetPrev;

        specialUp.Enable();
        specialDown.Enable();
        specialLeft.Enable();
        specialRight.Enable();
        targetNext.Enable();
        targetPrev.Enable();
    }

    void OnDisable()
    {
        move.Disable();

        interact.Disable();

        specialUp.Disable();
        specialDown.Disable();
        specialLeft.Disable();
        specialRight.Disable();
        targetNext.Disable();
        targetPrev.Disable();
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
            
            if (targetNext.WasPressedThisFrame() && isTargeting == false)
            {
                isTargeting = true;
                EnemyTargeting();
            }
            else if (yourEnemiesInRange.Count <= 0)
            {
                isTargeting = false;
            }

            PlayerStates();
            if (targetedEnemy != null)
            {
                SpecialInput();
            }
            else return;
        }
        else
        {
            SceneManager.LoadScene("DeathScene");
            Destroy(gameObject);
        }
    }
    private void FixedUpdate()
    {
        rb.velocity = moveDirection.normalized * speed;
    }

    private void Interact(InputAction.CallbackContext context)
    {
        Debug.Log("Interacted");
        if (yourEnemiesInRange.Count > 0)
        {
            Debug.Log("Battle Start");
            state = State.Battle;
        }
    }

    void PlayerActions()
    {
        moveDirection = move.ReadValue<Vector2>();
    }
    public void SpecialInput()
    {
        float specialDMG = 0f;
        if (specialUp.WasPressedThisFrame())
        {
            Debug.Log("DPAD UP Pressed");
            specialDMG = specials[0].SpecialAttack();
        }
        else if (specialRight.WasPressedThisFrame())
        {
            Debug.Log("DPAD RIGHT Pressed");
            //specialDMG = specials[1].SpecialAttack();
        }
        else if (specialDown.WasPressedThisFrame())
        {
            Debug.Log("DPAD DOWN Pressed");
            //specialDMG = specials[2].SpecialAttack();
        }
        else if (specialLeft.WasPressedThisFrame())
        {
            Debug.Log("DPAD LEFT Pressed");
            //specialDMG = specials[3].SpecialAttack();
        }
        targetedEnemy.health -= specialDMG;
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

            if (targetNext.WasPressedThisFrame())
            {
                enemyIndex = (enemyIndex + 1) % yourEnemiesInRange.Count;
            }
            else if (targetPrev.WasPressedThisFrame())
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

        if (other.gameObject.tag == "Debug")
        {
            HP = 0;
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
