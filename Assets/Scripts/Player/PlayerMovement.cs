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

    private MobileInput _mobileInput;
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
    private static readonly int DieBool = Animator.StringToHash("Die");
    private static readonly int HitTrigger = Animator.StringToHash("Hit");
    private static readonly int RollTrigger = Animator.StringToHash("Roll");


    private void Start()
    {
        UpdateHealthUI();
        _mobileInput = FindFirstObjectByType<MobileInput>();
        animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _audioSource = GetComponent<AudioSource>();
        _playerCollider = GetComponent<Collider2D>();
        animator.SetBool(DieBool, false);
    }

    private void Update()
    {
        if (_isRolling || _isDead) return;
        
        var horizontalInput = _mobileInput ? _mobileInput.horizontal : Input.GetAxis("Horizontal");
        _movement.x = horizontalInput * moveSpeed;
        transform.Translate(Time.deltaTime * _movement.x, 0, 0);
        CheckMovementInput();

        var rollPressed = (_mobileInput && _mobileInput.roll) || Input.GetKeyDown(KeyCode.LeftShift);
        if (rollPressed)
        {
            Roll(animator);
            if (_mobileInput) _mobileInput.ReleaseRoll();
        }

        var jumpPressed = (_mobileInput && _mobileInput.jump) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W);
        if (!_isGrounded || !jumpPressed) return;

        Jump();
        if (_mobileInput) _mobileInput.ReleaseJump();
    }

    private void CheckMovementInput()
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

    private void Roll(Animator animatorComponent)
    {
        _isRolling = true;
        animatorComponent.SetTrigger(RollTrigger);
        _audioSource.PlayOneShot(powerSound);

        var rollDirection = _spriteRenderer.flipX ? new Vector2(-1, 0.5f) : new Vector2(1, 0.5f);
        _rigidbody2d.AddForce(rollDirection.normalized * 2f, ForceMode2D.Impulse);
        
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemies)
        {
            var enemyCollider = enemy.GetComponent<Collider2D>();
            if (enemyCollider is not null)
            {
                Physics2D.IgnoreCollision(_playerCollider, enemyCollider, true);
            }
        }

        Invoke(nameof(EndRoll), 1.4f);
    }

    private void EndRoll()
    {
        _isRolling = false;
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemies)
        {
            var enemyCollider = enemy.GetComponent<Collider2D>();
            if (enemyCollider is not null)
            {
                Physics2D.IgnoreCollision(_playerCollider, enemyCollider, false);
            }
        }
    }

    private void Jump()
    {
        if ((!_mobileInput || !_mobileInput.jump) &&
            ((!Input.GetKeyDown(KeyCode.UpArrow) && !Input.GetKeyDown(KeyCode.W)) || _jump)) return;

        _rigidbody2d.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        _audioSource.PlayOneShot(jumpSound);
        _jump = true;
        _isGrounded = false;

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
        if (!collision.gameObject.CompareTag("Ground")) return;

        _jump = true;
        _isGrounded = false;
    }
    
    private void TakeDamage()
    {
        if (_isDead) return;

        health--;
        animator.SetTrigger(HitTrigger);
        UpdateHealthUI();

        if (health <= 0)
        {
            _isDead = true;
            animator.SetBool(RunBool, false);
            animator.SetBool(DieBool, true);
            StartCoroutine(LoadDeathScreenAfterDelay());
        }

        var knockbackDirection = _spriteRenderer.flipX ? new Vector2(1, 0.75f) : new Vector2(-1, 0.75f);
        _rigidbody2d.AddForce(knockbackDirection.normalized * 1.75f, ForceMode2D.Impulse);

        var playerCollider = GetComponent<Collider2D>();
        var enemyCollider = Physics2D.OverlapCircle(transform.position, 1f, LayerMask.GetMask($"Enemy"));
        if (enemyCollider is null) return;

        Physics2D.IgnoreCollision(playerCollider, enemyCollider, true);
        StartCoroutine(ReenableCollision(playerCollider, enemyCollider));
    }
    
    private static IEnumerator LoadDeathScreenAfterDelay()
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

    private static IEnumerator ReenableCollision(Collider2D playerCollider, Collider2D enemyCollider)
    {
        yield return new WaitForSeconds(5f);
        Physics2D.IgnoreCollision(playerCollider, enemyCollider, false);
    }
}