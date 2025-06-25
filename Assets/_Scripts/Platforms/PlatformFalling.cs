using UnityEngine;

public class PlatformFalling : MonoBehaviour {
    [Header("Timing")]
    [SerializeField] private float fallDelay = 0.5f;
    [SerializeField] private float destroyDelay = 2f;

    private Rigidbody2D rb;
    private bool isTriggered = false;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (!isTriggered && collision.collider.CompareTag("Player")) {
            isTriggered = true;
            Invoke(nameof(Fall), fallDelay);
        }
    }

    private void Fall() {
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 2f;
        Invoke(nameof(DestroyPlatform), destroyDelay);
    }

    private void DestroyPlatform() {
        Destroy(gameObject);
    }
}
