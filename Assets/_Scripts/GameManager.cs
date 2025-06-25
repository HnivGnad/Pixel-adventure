using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public static GameManager Instance; // Singleton để truy cập từ bất kỳ đâu

    [Header("Score Settings")]
    public int score = 0; // Điểm hiện tại
    public Text scoreText; // Text UI để hiển thị điểm

    private void Awake() {
        // Singleton pattern: Đảm bảo chỉ có một GameManager duy nhất
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    private void Start() {
        UpdateScoreUI();
    }

    // Hàm cộng điểm
    public void AddScore(int points) {
        score += points;
        UpdateScoreUI();
    }

    // Cập nhật UI khi thay đổi điểm
    private void UpdateScoreUI() {
        if (scoreText != null) {
            scoreText.text = "Score: " + score.ToString();
        }
    }
}
