using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    [Header("Combat")]
    public List<SpecialAttacks> specials = new List<SpecialAttacks>();
    public SpecialAttacks specialBeta;
    public float dmgLowEnd = 1f;
    public float dmgHighEnd = 3f;
    public int critDC = 20;
    public float critMultiplier = 1.5f;
    public int pBonus = 5;
    public int level = 1;
    public float cooldown = 2;
    private float timer;

    [Header("Targeting")]
    public List<EnemyBase> yourEnemiesInRange = new List<EnemyBase>();
    public EnemyBase targetedEnemy;
    public bool isTargeting = false;

    private float attackTimer = 0f;
    private int enemyIndex = 0;

    private InputAction specialUp;
    private InputAction specialDown;
    private InputAction specialLeft;
    private InputAction specialRight;
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

    private void OnDisable()
    {
        specialUp.Disable();
        specialDown.Disable();
        specialLeft.Disable();
        specialRight.Disable();
        targetNext.Disable();
        targetPrev.Disable();
    }

    private void Start()
    {
        specials.Add(specialBeta);
    }

    private void Update()
    {
        if (timer > 0){
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

        if (targetedEnemy != null)
            SpecialInput();
    }

    public void SpecialInput()
    {
        float specialDMG = 0f;
        if (specialUp.WasPressedThisFrame()) specialDMG = specials[0].SpecialAttack();
        else if (specialRight.WasPressedThisFrame()) specialDMG = specials[1].SpecialAttack();
        else if (specialDown.WasPressedThisFrame()) specialDMG = specials[2].SpecialAttack();
        else if (specialLeft.WasPressedThisFrame()) specialDMG = specials[3].SpecialAttack();

        if (targetedEnemy != null)
            targetedEnemy.health -= specialDMG;
    }

    public void Attack()
    {
        if (timer <= 0){
            anim.SetBool("Attacking", true);
            timer = cooldown;
        }
        
    }

    public void FinishedAttacking(){
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
}
