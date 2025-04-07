using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    private Rigidbody2D rigidbody2d;
    public Animator animator;
    private bool jump = false;
    private bool isGrounded;
    private Vector2 movement;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    public AudioClip jumpSound;
    public AudioClip hurtSound;
    public AudioClip tapSound;
    public AudioClip powerSound;


    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogWarning("AudioSource não encontrado no GameObject do Player.");
        }
    }
    
    void Update()
    {
        movement.x = Input.GetAxis("Horizontal") * moveSpeed;
        transform.Translate(Time.deltaTime * movement.x, 0, 0);
        
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

    // void FixedUpdate()
    // {
    //     rigidbody2d.MovePosition(rigidbody2d.position + movement * (moveSpeed * Time.fixedDeltaTime));
    //     if (isGrounded && (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)))
    //     {
    //         Jump();
    //     }
    // }
    
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
        audioSource.PlayOneShot(powerSound);
    }

    void Jump()
    {
        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && !jump)
        {
            rigidbody2d.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            audioSource.PlayOneShot(jumpSound);
            jump = true;
            isGrounded = false;
        }
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground") {
            jump = false;
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            jump = true;
            isGrounded = false;
        }
    }
    
}