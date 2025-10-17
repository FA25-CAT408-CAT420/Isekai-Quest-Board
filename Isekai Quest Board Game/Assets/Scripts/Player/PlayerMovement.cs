using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Inputs")]
    public PlayerInputActions playerControls;
    private InputAction move;
    private InputAction run;
    private InputAction attack;

    [Header("Movement")]
    public float gridSize = 1f;
    public float walkSpeed = 15f;
    public float runSpeed = 25f;
    public float currentSpeed;
    private Vector2 moveDirection;
    private bool isMoving = false;
    public Rigidbody2D rb;
    public bool isBlocked = false;

    public SpriteRenderer spriteRenderer;
    public Animator anim;
    private Vector2 lastMoveDirection = Vector2.zero;

    [Header("Scripts")]
    public PlayerCombat playerCombat;

    [Header("AttackPoint")]
    public Vector2 pos1;
    public int pos2;

    public LayerMask wallLayer;
    float rayDistance = 0.8f;

    private void Awake()
    {
        playerControls = new PlayerInputActions();
    }

    private void OnEnable()
    {
        move = playerControls.Player.Move;
        run = playerControls.Player.Run;
        attack = playerControls.Player.Attack;

        move.Enable();
        run.Enable();
        attack.Enable();

        run.performed += ctx => currentSpeed = runSpeed;
        run.canceled += ctx => currentSpeed = walkSpeed;
    }

    private void OnDisable()
    {
        move.Disable();
        run.Disable();
        attack.Disable();

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

        if (!isMoving){
            HandleMovement();
        }
        anim.SetBool("Moving", isMoving);

        if (attack.WasPressedThisFrame())
        {
            playerCombat.Attack();
        }

        RaycastHit2D ray = Physics2D.Raycast(transform.position, pos1, rayDistance, wallLayer);

        if (ray.collider != null)
        {
            isBlocked = true;

        }
        else
        {
            isBlocked = false;
        }
        
        Debug.DrawRay(transform.position, pos1 * rayDistance, Color.red);
    }

    private void Animator(){
        if (moveDirection.x < -0.1f)
        {
            spriteRenderer.flipX = true; 
        }
        else if (moveDirection.x > 0.1f)
        {
            spriteRenderer.flipX = false;
        }
    }
    
    //Takes in magnitude of players input and breaks it down to whole numbers. e.g. (1,0) (1,1)
    private void HandleMovement()
    {

        //Players maginitude e.g(0.089, 2.095)
        moveDirection = move.ReadValue<Vector2>();

        //Checks the magnitude and hard sets it to values based on 1
        if (moveDirection.magnitude > 0.1f) // dead zone
        {
            if (Mathf.Abs(moveDirection.x) > Mathf.Abs(moveDirection.y))
            {
                moveDirection = new Vector2(Mathf.Sign(moveDirection.x), 0);
                Debug.Log(moveDirection);
                pos1 = moveDirection;
            }
            else
            {
                moveDirection = new Vector2(0, Mathf.Sign(moveDirection.y));
                Debug.Log(moveDirection);
                pos1 = moveDirection;
            }

            //takes player transform and adds players magnitute to transform coordinates
            var targetPos = transform.position;
            targetPos += (Vector3)moveDirection;

            anim.SetFloat("X", moveDirection.x);
            anim.SetFloat("Y", moveDirection.y * -1);

            //Inputs new coordinates into coroutine to transform players position
            StartCoroutine(Move(targetPos));
        }


    }

    //transforms players position with math
    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;
        Vector3 originalPosition = transform.position;

        if (!isBlocked)
        {
            while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, currentSpeed * Time.deltaTime);
                yield return null;
            }
            transform.position = targetPos;
        }
        else if (isBlocked)
        {
            transform.position = originalPosition;
            isBlocked = false;
        }
        
        isMoving = false;
    }
    
}
