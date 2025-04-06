using UnityEngine;

public class EnemyWalker : MonoBehaviour
{
    public float speed = 1f;
    public bool movingRight;
    public Animator animator;

    private Rigidbody2D rb;
    private string spawnAnimationName;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        
        string controllerName = animator.runtimeAnimatorController.name;
        spawnAnimationName = controllerName.Contains("Green") ? "Green_Slime_Spawn" : "Purple_Slime_Spawn";
    }

    void FixedUpdate()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(spawnAnimationName))
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 direction = movingRight ? Vector2.right : Vector2.left;
        rb.linearVelocity = new Vector2(direction.x * speed, rb.linearVelocity.y);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyLimit"))
        {
            movingRight = !movingRight;

            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }
}