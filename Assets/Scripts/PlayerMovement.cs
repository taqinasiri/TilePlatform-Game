using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float jumpSpeed = 5f;
    [SerializeField] private float climbSpeed = 5f;
    [SerializeField] private Vector2 deathKick = new(10f,10f);
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform gun;

    private Vector2 moveInput;
    private Rigidbody2D rigidBody;
    private Animator animator;
    private CapsuleCollider2D bodyCapsuleCollider;
    private BoxCollider2D feetBoxCollider;

    private float gravityScaleAtStart;

    private bool isAlive = true;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        bodyCapsuleCollider = GetComponent<CapsuleCollider2D>();
        feetBoxCollider = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = rigidBody.gravityScale;
    }

    private void Update()
    {
        if(!isAlive)
        {
            return;
        }
        Run();
        FlipSprite();
        ClimbLadder();
        Die();
    }

    #region Input System

    private void OnMove(InputValue inputValue)
    {
        if(!isAlive)
        {
            return;
        }
        moveInput = inputValue.Get<Vector2>();
    }

    private void OnJump(InputValue inputValue)
    {
        if(!isAlive)
        {
            return;
        }
        if(!feetBoxCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
            return;

        if(inputValue.isPressed)
            rigidBody.velocity += new Vector2(0,jumpSpeed);
    }

    private void OnFire(InputValue inputValue)
    {
        if(!isAlive)
        {
            return;
        }
        if(inputValue.isPressed)
        {
            Instantiate(bullet,gun.position,transform.rotation);
        }
    }

    #endregion Input System

    private void Run()
    {
        Vector2 playerVolocity = new(moveInput.x * runSpeed,rigidBody.velocity.y);
        rigidBody.velocity = playerVolocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(rigidBody.velocity.x) > Mathf.Epsilon;
        animator.SetBool("isRunning",playerHasHorizontalSpeed);
    }

    private void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(rigidBody.velocity.x) > Mathf.Epsilon;

        if(playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(rigidBody.velocity.x),1f);
        }
    }

    private void ClimbLadder()
    {
        if(!feetBoxCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            rigidBody.gravityScale = gravityScaleAtStart;
            return;
        }

        Vector2 climbVolocity = new(rigidBody.velocity.x,moveInput.y * climbSpeed);
        rigidBody.velocity = climbVolocity;
        rigidBody.gravityScale = 0;

        bool playerHasVerticalSpeed = Mathf.Abs(rigidBody.velocity.y) > Mathf.Epsilon;
        animator.SetBool("isClimbing",playerHasVerticalSpeed);
    }

    private void Die()
    {
        if(bodyCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Enemies","Hazards")))
        {
            isAlive = false;
            animator.SetTrigger("Dying");
            rigidBody.velocity = deathKick;
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }
}