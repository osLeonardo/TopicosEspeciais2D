using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Animator animator;

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
        movement.y = Input.GetAxisRaw("Vertical");
        
        CheckMovementInput();
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("Roll");
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
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
}