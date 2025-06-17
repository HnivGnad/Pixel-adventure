using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public int maxJumps = 2;

    private int jumpCount = 0;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update() {
        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        if (moveInput != 0) {
            spriteRenderer.flipX = moveInput < 0;
        }

        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < maxJumps) {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount++;
        }
    }

    void OnCollisionEnter2D(Collision2D other) {
        if (other.collider.CompareTag("Ground")) {
            jumpCount = 0;
        }
    }
}
