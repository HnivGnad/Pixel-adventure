using UnityEngine;
using UnityEngine.SceneManagement;  // Để quản lý các scene trong game

public class GameOverMenu : MonoBehaviour {
    // Hàm Restart sẽ load lại scene hiện tại
    public void RestartGame() {
        // Load lại scene hiện tại
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;  // Đảm bảo thời gian trò chơi tiếp tục
    }

    // Hàm Exit sẽ thoát game
    public void ExitGame() {
        // Thoát game (chỉ hoạt động khi build game, không hoạt động trong editor)
        Application.Quit();
        Debug.Log("Game Exited");
    }
}
