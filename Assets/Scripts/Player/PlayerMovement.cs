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

    private bool _isDead;
    private bool _isRolling;
    private bool _jump;
    private bool _isGrounded;
    private Vector2 _movement;
    private Rigidbody2D _rigidbody2d;
    private SpriteRenderer _spriteRenderer;
    private AudioSource _audioSource;
    private Collider2D _playerCollider;

    private static readonly int RunBool = Animator.StringToHash("Run");
    private static readonly int HitTrigger = Animator.StringToHash("Hit");
    private static readonly int DieTrigger = Animator.StringToHash("Die");
    private static readonly int RollTrigger = Animator.StringToHash("Roll");


    void Start()
    {
        UpdateHealthUI();
        animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _audioSource = GetComponent<AudioSource>();
        _playerCollider = GetComponent<Collider2D>();
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
        if (_movement.x is not 0)
        {
            animator.SetBool(RunBool, true);
            _spriteRenderer.flipX = _movement.x < 0;
        }
        else
        {
            animator.SetBool(RunBool, false);
        }
    }

    void Roll(Animator animatorComponent)
    {
        _isRolling = true;
        animatorComponent.SetTrigger(RollTrigger);
        _audioSource.PlayOneShot(powerSound);

        Vector2 rollDirection = _spriteRenderer.flipX ? new Vector2(-1, 0.5f) : new Vector2(1, 0.5f);
        _rigidbody2d.AddForce(rollDirection.normalized * 2f, ForceMode2D.Impulse);
        
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Collider2D enemyCollider = enemy.GetComponent<Collider2D>();
            if (enemyCollider is not null)
            {
                Physics2D.IgnoreCollision(_playerCollider, enemyCollider, true);
            }
        }

        Invoke("EndRoll", 1.4f);
    }

    void EndRoll()
    {
        _isRolling = false;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Collider2D enemyCollider = enemy.GetComponent<Collider2D>();
            if (enemyCollider is not null)
            {
                Physics2D.IgnoreCollision(_playerCollider, enemyCollider, false);
            }
        }
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
        if (collision.gameObject.CompareTag("Ground"))
        {
            _jump = false;
            _isGrounded = true;
        }
        else if (collision.gameObject.CompareTag("Enemy"))
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
        if (collision.gameObject.CompareTag("Ground"))
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
            animator.SetTrigger(HitTrigger);
            UpdateHealthUI();

            Vector2 knockbackDirection = _spriteRenderer.flipX ? new Vector2(1, 0.75f) : new Vector2(-1, 0.75f);
            _rigidbody2d.AddForce(knockbackDirection.normalized * 1.75f, ForceMode2D.Impulse);

            Collider2D playerCollider = GetComponent<Collider2D>();
            Collider2D enemyCollider = Physics2D.OverlapCircle(transform.position, 1f, LayerMask.GetMask($"Enemy"));
            if (enemyCollider is not null)
            {
                Physics2D.IgnoreCollision(playerCollider, enemyCollider, true);
                StartCoroutine(ReenableCollision(playerCollider, enemyCollider));
            }
        }

        if (health <= 0)
        {
            _isDead = true;
            animator.SetTrigger(DieTrigger);
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
}