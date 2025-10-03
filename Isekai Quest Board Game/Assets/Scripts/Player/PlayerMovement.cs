using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Inputs")]
    public PlayerInputActions playerControls;
    private InputAction move;
    private InputAction run;

    [Header("Movement")]
    public float gridSize = 1f;
    public float walkSpeed = 15f;
    public float runSpeed = 25f;
    public float currentSpeed;
    private Vector2 moveDirection;
    private bool isMoving = false;
    private Rigidbody2D rb;

    public SpriteRenderer spriteRenderer;
    public Animator anim;
    private Vector2 lastMoveDirection = Vector2.zero;

    private void Awake()
    {
        playerControls = new PlayerInputActions();
    }

    private void OnEnable()
    {
        move = playerControls.Player.Move;
        run = playerControls.Player.Run;

        move.Enable();
        run.Enable();

        run.performed += ctx => currentSpeed = runSpeed;
        run.canceled += ctx => currentSpeed = walkSpeed;
    }

    private void OnDisable()
    {
        move.Disable();
        run.Disable();

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
    private void HandleMovement()
    {
        moveDirection = move.ReadValue<Vector2>();

        if (moveDirection.magnitude > 0.1f) // dead zone
        {
            if (Mathf.Abs(moveDirection.x) > Mathf.Abs(moveDirection.y))
            {
                moveDirection = new Vector2(Mathf.Sign(moveDirection.x), 0);
            }
            else
            {
                moveDirection = new Vector2(0, Mathf.Sign(moveDirection.y));
            }

            var targetPos = transform.position;
            targetPos += (Vector3)moveDirection;

            anim.SetFloat("X", moveDirection.x);
            anim.SetFloat("Y", moveDirection.y * -1);

            StartCoroutine(Move(targetPos));
        }
    }

    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;
        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon){
            transform.position = Vector3.MoveTowards(transform.position, targetPos, currentSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;
        isMoving = false;
    }
}
