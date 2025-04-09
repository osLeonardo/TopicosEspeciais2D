using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public int health = 3;
    public float moveSpeed;
    public float jumpForce;
    public Animator animator;
    public AudioClip jumpSound;
    public AudioClip hurtSound;
    public AudioClip tapSound;
    public AudioClip powerSound;
    public TextMeshProUGUI healthText;
    public GameObject deathScreen;

    private bool _isDead;
    private bool _isRolling;
    private bool _jump;
    private bool _isGrounded;
    private Vector2 _movement;
    private Rigidbody2D _rigidbody2d;
    private SpriteRenderer _spriteRenderer;
    private AudioSource _audioSource;


    void Start()
    {
        UpdateHealthUI();
        animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _audioSource = GetComponent<AudioSource>();
    }
    
    void Update()
    {
        if (_isRolling || _isDead) return;
        
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
        _isRolling = true;
        animator.SetTrigger("Roll");
        _audioSource.PlayOneShot(powerSound);
        
        Vector2 rollDirection = _spriteRenderer.flipX ? new Vector2(-1, 0.5f) : new Vector2(1, 0.5f);
        _rigidbody2d.AddForce(rollDirection.normalized * 2f, ForceMode2D.Impulse);

        Collider2D playerCollider = GetComponent<Collider2D>();
        Collider2D[] enemyColliders = Physics2D.OverlapCircleAll(transform.position, 1f, LayerMask.GetMask("Enemy"));

        foreach (var enemyCollider in enemyColliders)
        {
            Physics2D.IgnoreCollision(playerCollider, enemyCollider, true);
        }

        StartCoroutine(ReenableCollisionAfterRoll(playerCollider, enemyColliders));
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
                TakeDamage();
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
    
    private void TakeDamage()
    {
        if (!_isDead)
        {
            health--;
            animator.SetTrigger("Hit");
            UpdateHealthUI();

            Vector2 knockbackDirection = _spriteRenderer.flipX ? new Vector2(1, 0.75f) : new Vector2(-1, 0.75f);
            _rigidbody2d.AddForce(knockbackDirection.normalized * 1.75f, ForceMode2D.Impulse);

            Collider2D playerCollider = GetComponent<Collider2D>();
            Collider2D enemyCollider = Physics2D.OverlapCircle(transform.position, 1f, LayerMask.GetMask("Enemy"));
            if (enemyCollider != null)
            {
                Physics2D.IgnoreCollision(playerCollider, enemyCollider, true);
                StartCoroutine(ReenableCollision(playerCollider, enemyCollider));
            }
        }

        if (health <= 0)
        {
            _isDead = true;
            animator.SetTrigger("Die");
            StartCoroutine(LoadDeathScreenAfterDelay());
        }
    }
    
    IEnumerator LoadDeathScreenAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("DeathScreen");
    }

    private void UpdateHealthUI()
    {
        if (healthText is not null)
        {
            healthText.text = "Lives: 0" + health;
        }
    }

    private IEnumerator ReenableCollision(Collider2D playerCollider, Collider2D enemyCollider)
    {
        yield return new WaitForSeconds(5f);
        Physics2D.IgnoreCollision(playerCollider, enemyCollider, false);
    }
    
    private IEnumerator ReenableCollisionAfterRoll(Collider2D playerCollider, Collider2D[] enemyColliders)
    {
        yield return new WaitForSeconds((float)(animator.GetCurrentAnimatorStateInfo(0).length * 0.6));

        foreach (var enemyCollider in enemyColliders)
        {
            if (enemyCollider is not null)
            {
                Physics2D.IgnoreCollision(playerCollider, enemyCollider, false);
            }
        }
        
        _isRolling = false;
    }
}