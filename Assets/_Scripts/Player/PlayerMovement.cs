using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour {
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float jumpForce = 16f;

    [Header("Double Jump Settings")]
    [SerializeField] private int maxJumps = 1;
    [SerializeField] private float secondJumpMultiplier = 0.8f;

    [Header("Wall Jump Settings")]
    [SerializeField] private float wallSlideSpeed = 2f;
    [SerializeField] private Vector2 wallJumpForce = new(3f, 14f);
    [SerializeField] private float wallJumpTime = 0.2f;
    [SerializeField] private float wallJumpDuration = 0.3f;

    [Header("Check Settings")]
    [SerializeField] private Vector2 groundCheckSize = new(0.5f, 0.1f);
    [SerializeField] private float wallCheckRadius = 0.3f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask platformLayer;
    [SerializeField] private LayerMask wallLayer;

    [Header("Dust Effect")]
    [SerializeField] private GameObject dustEffectPrefab;
    [SerializeField] private Transform dustSpawnPoint;

    private Rigidbody2D rb;
    private PlayerAnimationHandler anim;

    private float horizontalInput;
    private bool isFacingRight = true;
    private bool isWallSliding;
    private bool isWallJumping;
    private float wallJumpCounter;
    private float wallJumpDirection;
    private bool isGrounded;
    private bool wasGrounded = false;
    private int jumpCount;
    [SerializeField]private GameObject gameOverMenu;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<PlayerAnimationHandler>();
    }

    private void Update() {
        ReadInput();

        wasGrounded = isGrounded;
        isGrounded = IsGrounded();

        if (!wasGrounded && isGrounded) {
            CreateDust();
        }

        if (isGrounded && !isWallJumping) {
            jumpCount = 0;
        }

        HandleJump();
        WallSlide();
        WallJump();

        if (!isWallJumping)
            Flip();
    }

    private void FixedUpdate() {
        if (!isWallJumping)
            rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
    }

    private void ReadInput() {
        horizontalInput = Input.GetAxisRaw("Horizontal");
    }

    private void HandleJump() {
        if (Input.GetButtonDown("Jump")) {
            if (isGrounded) {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpCount = 1;
                CreateDust();
            }
            else if (jumpCount < maxJumps) {
                float jumpStrength = jumpForce * secondJumpMultiplier;
                rb.velocity = new Vector2(rb.velocity.x, jumpStrength);
                jumpCount++;
                if (anim != null) anim.TriggerDoubleJump();
            }
            else if (isWallSliding) {
                wallJumpDirection = -transform.localScale.x;
                rb.velocity = new Vector2(wallJumpDirection * wallJumpForce.x, wallJumpForce.y);
                isWallJumping = true;
                wallJumpCounter = 0f;
                jumpCount = 1;

                if (transform.localScale.x != wallJumpDirection) {
                    FlipLocalScale();
                    isFacingRight = wallJumpDirection > 0f;
                }

                if (anim != null) anim.TriggerWallJump();
                Invoke(nameof(StopWallJumping), wallJumpDuration);
            }
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f) {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }

    private void StopWallJumping() {
        isWallJumping = false;
    }

    private void WallSlide() {
        if (IsWalled() && !IsGrounded()) {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
        }
        else {
            isWallSliding = false;
        }
    }

    private void WallJump() {
        if (isWallSliding) {
            isWallJumping = false;
            wallJumpCounter = wallJumpTime;
            CancelInvoke(nameof(StopWallJumping));
        }
        else {
            wallJumpCounter -= Time.deltaTime;
        }
    }

    // --- Va chạm ---
    private bool IsGrounded() {
        return Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0f, groundLayer | platformLayer);
    }

    private bool IsWalled() {
        return Physics2D.OverlapCircle(wallCheck.position, wallCheckRadius, wallLayer);
    }

    // --- Flip ---
    private void Flip() {
        if ((isFacingRight && horizontalInput < 0f) || (!isFacingRight && horizontalInput > 0f)) {
            isFacingRight = !isFacingRight;
            FlipLocalScale();
        }
    }

    private void FlipLocalScale() {
        Vector3 scale = transform.localScale;
        scale.x *= -1f;
        transform.localScale = scale;
    }

    // --- Hiệu ứng ---
    private void CreateDust() {
        if (dustEffectPrefab != null && dustSpawnPoint != null) {
            GameObject dust = Instantiate(dustEffectPrefab, dustSpawnPoint.position, Quaternion.identity);
            Destroy(dust, 1f);
        }
    }

    // --- Gizmos ---
    private void OnDrawGizmosSelected() {
        if (groundCheck != null) {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);
        }

        if (wallCheck != null) {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(wallCheck.position, wallCheckRadius);
        }
    }


    // --- Public cho Animator ---
    public bool IsOnGround() => isGrounded;
    public bool IsSlidingOnWall() => isWallSliding;
    // Hiển thị menu game over
    private void ShowGameOverMenu() {
        // Tạm dừng trò chơi
        Time.timeScale = 0f;

        // Hiển thị menu game over
        if (gameOverMenu != null) {
            gameOverMenu.SetActive(true);  // Đảm bảo menu game over hiển thị
        }
    }

    // Hiển thị menu game over khi nhân vật va chạm với vùng chết
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("DeathZone")) {
            ShowGameOverMenu();  // Hiển thị menu game over khi chạm vào vùng chết
        }
    }
}
