using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    public Animator animator;
    public AudioClip jumpSound;
    public AudioClip hurtSound;
    public AudioClip tapSound;
    public AudioClip powerSound;
    
    private bool _jump = false;
    private bool _isGrounded;
    private Vector2 _movement;
    private Rigidbody2D _rigidbody2d;
    private SpriteRenderer _spriteRenderer;
    private AudioSource _audioSource;


    void Start()
    {
        animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _audioSource = GetComponent<AudioSource>();
    }
    
    void Update()
    {
        _movement.x = Input.GetAxis("Horizontal") * moveSpeed;
        transform.Translate(Time.deltaTime * _movement.x, 0, 0);
        
        CheckMovementInput();
        
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Roll(animator);
        }
        
        if (_isGrounded && (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)))
        {
            Jump();
        }
    }

    void CheckMovementInput()
    {
        if (_movement.x != 0)
        {
            animator.SetBool("Run", true);
            _spriteRenderer.flipX = _movement.x < 0;
        }
        else
        {
            animator.SetBool("Run", false);
        }
    }

    void Roll(Animator animator)
    {
        animator.SetTrigger("Roll");
        _audioSource.PlayOneShot(powerSound);
    }

    void Jump()
    {
        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && !_jump)
        {
            _rigidbody2d.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            _audioSource.PlayOneShot(jumpSound);
            _jump = true;
            _isGrounded = false;
        }
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground") {
            _jump = false;
            _isGrounded = true;
        }
        else if (collision.gameObject.tag == "Enemy")
        {
            if (collision.contacts[0].normal.y > 0.5f)
            {
                collision.gameObject.GetComponent<EnemyWalker>().Die();
                _rigidbody2d.AddForce(new Vector2(0, jumpForce / 2), ForceMode2D.Impulse);
            }
            else
            {
                // Handle player damage or death
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            _jump = true;
            _isGrounded = false;
        }
    }
    
}