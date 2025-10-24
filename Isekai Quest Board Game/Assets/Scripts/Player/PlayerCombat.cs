using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    [Header("Scripts")]
    public PlayerMovement playerMovement;
    public PlayerHealth playerHealth;

    [Header("Hitboxing")]
    public Transform attackPoint;
    public float weaponRange = 1;
    public LayerMask enemyLayer;
    public int damage = 1;

    [Header("Combat")]
    public List<Spells> specials = new List<Spells>();
    public Spells specialBeta;
    public float dmgLowEnd = 1f;
    public float dmgHighEnd = 3f;
    public int critDC = 20;
    public float critMultiplier = 1.5f;
    public int pBonus = 5;
    public int level = 1;
    public float cooldown = 2;
    private float timer;
    public int knockbackForce = 1;
    public float takeDamage;

    [Header("Targeting")]
    public List<EnemyBase> yourEnemiesInRange = new List<EnemyBase>();
    public EnemyBase targetedEnemy;
    public bool isTargeting = false;

    private float attackTimer = 0f;
    private int enemyIndex = 0;

    private InputAction targetNext;
    private InputAction targetPrev;

    public PlayerInputActions playerControls;

    public Animator anim;

    private void Awake()
    {
        playerControls = new PlayerInputActions();
    }

    private void OnEnable()
    {
        targetNext = playerControls.Player.TargetNext;
        targetPrev = playerControls.Player.TargetPrev;

        targetNext.Enable();
        targetPrev.Enable();
    }

    private void OnDisable()
    {
        targetNext.Disable();
        targetPrev.Disable();
    }

    private void Start()
    {
        specials.Add(specialBeta);
        specials.Add(specialBeta);
        specials.Add(specialBeta);
        specials.Add(specialBeta);
    }

    private void Update()
    {
        attackPoint.position = playerMovement.rb.position + playerMovement.pos1;
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }

        if (targetNext.WasPressedThisFrame() && !isTargeting)
        {
            isTargeting = true;
            EnemyTargeting();
        }
        else if (yourEnemiesInRange.Count <= 0)
        {
            isTargeting = false;
        }
    }

    public void SpecialInput(int index)
    {   
        float mpCost = specials[index].CostCalculation();
        specials[index].Cost();
        
        if (playerHealth.MP >= mpCost){
            playerHealth.MP = playerHealth.MP - mpCost;
            specials[index].Spell();
        }
    }

    public void Attack()
    {
        if (timer <= 0)
        {
            anim.SetBool("Attacking", true);

            timer = cooldown;
        }
    }

public void DealDamage()
{
    Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPoint.position, weaponRange, enemyLayer);

        foreach (Collider2D enemy in enemies)
        {
            if (enemy.isTrigger) continue;

            playerMovement.isBlocked = true;

            if (enemies.Length > 0)
            {
                enemies[0].GetComponent<EnemyBase>().ChangeHealth(-damage);
                enemies[0].GetComponent<EnemyKnockback>().Knockback(transform, knockbackForce);
                break;
            }

            // EnemyBase enemyBase = enemy.GetComponent<EnemyBase>();
            // EnemyKnockback enemyKnockback = enemy.GetComponent<EnemyKnockback>();

            // if (enemyBase != null)
            // {
            //     enemyBase.ChangeHealth(-damage);
            //     enemyKnockback.Knockback(transform, knockbackForce);
            //     break;
            // }
        }

        if (enemies == null)
        {
            playerMovement.isBlocked = false;
        }
}

// public void CastSpell()
// {
//     Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPoint.position, weaponRange, enemyLayer);

//         foreach (Collider2D enemy in enemies)
//         {
//             if (enemy.isTrigger) continue;

//             playerMovement.isBlocked = true;

//             if (enemies.Length > 0)
//             {
//                 enemies[0].GetComponent<EnemyBase>().ChangeHealth(-spellDamage);
//                 enemies[0].GetComponent<EnemyKnockback>().Knockback(transform, knockbackForce);
//                 break;
//             }
//         }

//         if (enemies == null)
//         {
//             playerMovement.isBlocked = false;
//         }
// }

    public void FinishedAttacking()
    {
        anim.SetBool("Attacking", false);
    }

    public void EnemyTargeting()
    {
        if (yourEnemiesInRange.Count <= 0) return;

        enemyIndex = Mathf.Clamp(enemyIndex, 0, yourEnemiesInRange.Count - 1);
        targetedEnemy = yourEnemiesInRange[enemyIndex];

        if (targetNext.WasPressedThisFrame()) enemyIndex = (enemyIndex + 1) % yourEnemiesInRange.Count;
        else if (targetPrev.WasPressedThisFrame())
        {
            enemyIndex--;
            if (enemyIndex < 0) enemyIndex = yourEnemiesInRange.Count - 1;
        }

        for (int i = 0; i < yourEnemiesInRange.Count; i++)
            yourEnemiesInRange[i].isTargeted = i == enemyIndex;
    }

    public void AddYourEnemyToList(EnemyBase enemy)
    {
        if (!yourEnemiesInRange.Contains(enemy))
            yourEnemiesInRange.Add(enemy);
    }

    public void RemoveYourEnemyFromList(EnemyBase enemy)
    {
        if (yourEnemiesInRange.Contains(enemy))
        {
            enemy.isTargeted = false;
            yourEnemiesInRange.Remove(enemy);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.red; // Circle color
        Gizmos.DrawWireSphere(attackPoint.position, weaponRange);
    }
    

}
