using UnityEngine;

public class EnemyWalker : MonoBehaviour
{
    public float speed = 1f;
    public bool movingRight = false;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
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