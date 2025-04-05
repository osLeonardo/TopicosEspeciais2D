using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 2.5f;
    public float jumpForce = 10f;
    public Rigidbody2D rigidbody2d;
    public Animator animator;

    private bool isGrounded;
    private Vector2 movement;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        
        CheckMovementInput();
        
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Roll(animator);
        }

        if (isGrounded && (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)))
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        rigidbody2d.MovePosition(rigidbody2d.position + movement * (moveSpeed * Time.fixedDeltaTime));
    }
    
    void CheckMovementInput()
    {
        if (movement.x != 0)
        {
            animator.SetBool("Run", true);
            spriteRenderer.flipX = movement.x < 0;
        }
        else
        {
            animator.SetBool("Run", false);
        }
    }

    void Roll(Animator animator)
    {
        animator.SetTrigger("Roll");
    }

    void Jump()
    {
        rigidbody2d.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        isGrounded = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}