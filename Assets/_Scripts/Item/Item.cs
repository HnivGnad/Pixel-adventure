using UnityEngine;

public class Item : MonoBehaviour {
    [Header("Item Settings")]
    [SerializeField] private int points = 10; // Điểm cộng khi nhặt item

    // Sự kiện khi người chơi chạm vào item
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            // Gọi hàm cộng điểm
            GameManager.Instance.AddScore(points);

            // Hủy item sau khi nhặt
            Destroy(gameObject);
        }
    }
}
