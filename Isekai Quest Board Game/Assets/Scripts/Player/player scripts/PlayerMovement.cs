using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Inputs")]
    public PlayerInputActions playerControls;
    public InputAction move;
    private InputAction run;
    private InputAction attack;
    // specials
    private InputAction specialUp;
    private InputAction specialDown;
    private InputAction specialLeft;
    private InputAction specialRight;

    int nextSpell = 0;

    [Header("Movement")]
    public float gridSize = 1f;
    public float walkSpeed = 15f;
    public float runSpeed = 25f;
    public float currentSpeed;
    private Vector2 moveDirection;
    public bool isMoving = false;
    public Rigidbody2D rb;
    private Vector3 originalPosition;
    public bool isBlocked = false;
    public Coroutine moveCoroutine;

    public SpriteRenderer spriteRenderer;
    public Animator anim;

    [Header("Scripts")]
    public PlayerCombat playerCombat;

    [Header("AttackPoint")]
    public Vector2 pos1;
    public LayerMask wallLayer;
    public LayerMask enemyLayer;
    public float rayDistance = 0.7f;

    private void Awake()
    {
        playerControls = new PlayerInputActions();
    }

    private void OnEnable()
    {
        move = playerControls.Player.Move;
        run = playerControls.Player.Run;
        attack = playerControls.Player.Attack;
        // specials
        specialUp = playerControls.Player.SpecialUp;
        specialDown = playerControls.Player.SpecialDown;
        specialLeft = playerControls.Player.SpecialLeft;
        specialRight = playerControls.Player.SpecialRight;

        move.Enable();
        run.Enable();
        attack.Enable();
        
        //special enable
        specialUp.Enable();
        specialDown.Enable();
        specialLeft.Enable();
        specialRight.Enable();

        run.performed += ctx => currentSpeed = runSpeed;
        run.canceled += ctx => currentSpeed = walkSpeed;
    }

    private void OnDisable()
    {
        move.Disable();
        run.Disable();
        attack.Disable();

        // special disable
        specialUp.Disable();
        specialDown.Disable();
        specialLeft.Disable();
        specialRight.Disable();

        run.performed -= ctx => currentSpeed = runSpeed;
        run.canceled -= ctx => currentSpeed = walkSpeed;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentSpeed = walkSpeed;
    }

    private void Update()
    {
        Animator();

        if (!isMoving)
        {
            HandleMovement();
        }

        anim.SetBool("Moving", isMoving);

        if (attack.WasPressedThisFrame())
        {
            playerCombat.Attack();
        }

        if (attack.WasPressedThisFrame())
        {
            playerCombat.Attack();
        }
        
        if (specialUp.WasPressedThisFrame())
            {
                if (playerCombat.specials.Count > 0)
                {
                    playerCombat.SpecialInput(nextSpell);
                    
                }
                else if (playerCombat.specials.Count <= 0)
                {
                    Debug.Log("You got no spells cuh");
                }
                
            }
        
        if (specialLeft.WasPressedThisFrame()){
            nextSpell--;
        }
        else if (specialRight.WasPressedThisFrame()){
            nextSpell++;
        }

        if (nextSpell >= playerCombat.specials.Count)
        {
            nextSpell = 0;
        }
        else if (nextSpell < 0)
        {
            nextSpell = playerCombat.specials.Count - 1;
        }
        
        Debug.DrawRay(transform.position, pos1 * rayDistance, Color.red);
    }

    private void Animator()
    {
        if (moveDirection.x < -0.1f)
            spriteRenderer.flipX = true;
        else if (moveDirection.x > 0.1f)
            spriteRenderer.flipX = false;
    }

    private void HandleMovement()
    {
        moveDirection = move.ReadValue<Vector2>();
        

        if (moveDirection.magnitude > 0.1f)
        {
            // Cardinalize direction
            if (Mathf.Abs(moveDirection.x) > Mathf.Abs(moveDirection.y))
                moveDirection = new Vector2(Mathf.Sign(moveDirection.x), 0);
            else
                moveDirection = new Vector2(0, Mathf.Sign(moveDirection.y));

            pos1 = moveDirection;

            // Raycast in the direction we're about to move
            RaycastHit2D hit = Physics2D.Raycast(transform.position, moveDirection, rayDistance, wallLayer);
            isBlocked = hit.collider != null;

            if (isBlocked)
            {
                Debug.Log("Blocked movement in direction: " + moveDirection);
                return; // don’t start coroutine
            }

            Vector3 targetPos = transform.position + (Vector3)moveDirection;
            anim.SetFloat("X", moveDirection.x);
            anim.SetFloat("Y", moveDirection.y * -1);
            originalPosition = transform.position;

            moveCoroutine = StartCoroutine(Move(targetPos));
        }
    }

    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            // Raycast mid-move in the same direction
            RaycastHit2D hit = Physics2D.Raycast(transform.position, moveDirection, rayDistance, wallLayer);
            if (hit.collider != null)
            {
                Debug.Log("Hit da wall.");
                break; // Stop immediately when a wall is detected
            }

            transform.position = Vector3.MoveTowards(transform.position, targetPos, currentSpeed * Time.deltaTime);
            yield return null;
        }

        //  Snap to current position (not targetPos) to avoid teleport past wall
        transform.position = new Vector3(
            Mathf.Round(transform.position.x),
            Mathf.Round(transform.position.y),
            Mathf.Round(transform.position.z)
        );

        isMoving = false;
    }

    public void StopMovementCoroutine()
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
            moveCoroutine = null;
            isMoving = false; // make sure player can move again
        }
    }
}
