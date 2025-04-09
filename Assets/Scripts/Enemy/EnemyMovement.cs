using UnityEngine;

public class EnemyWalker : MonoBehaviour
{
    public float speed = 1f;
    public bool movingRight;
    public Animator animator;

    private Rigidbody2D _rb;
    private Collider2D _collider2d;
    private string _spawnAnimationName;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider2d = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        
        string controllerName = animator.runtimeAnimatorController.name;
        _spawnAnimationName = controllerName.Contains("Green") ? "Green_Slime_Spawn" : "Purple_Slime_Spawn";
    }

    void FixedUpdate()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(_spawnAnimationName))
        {
            _rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 direction = movingRight ? Vector2.right : Vector2.left;
        _rb.linearVelocity = new Vector2(direction.x * speed, _rb.linearVelocity.y);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyLimit") || collision.gameObject.CompareTag("Player"))
        {
            movingRight = !movingRight;

            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }
    
    public void Die()
    {
        animator.SetTrigger("Die");
        
        _rb.linearVelocity = Vector2.zero;
        _rb.bodyType = RigidbodyType2D.Kinematic;
        _collider2d.enabled = false;
        _rb.constraints = RigidbodyConstraints2D.FreezeAll;
        
        Destroy(gameObject, 1f);
    }
}