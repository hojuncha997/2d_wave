using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 게임의 상태(진행, 종료, 재시작)를 관리합니다.
/// </summary>
public class GameManager : MonoBehaviour
{
    // 어디환 곳에서나 접근할 수 있도록 싱글톤 구현
    public static GameManager Instance { get; private set; }

    public bool IsGameOver { get; private set; } = false;

    private void Awake()
    {
        // 중복 인스턴스 방지
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 플레이어 사망 시 호출됩니다.
    /// </summary>
    public void EndGame()
    {
        if (IsGameOver) return;

        IsGameOver = true;
        Debug.Log("게임 오버! 'R'키를 눌러 재시작하세요.");

        // 시간을 멈춰서 게임 정지 (선택 사항)
        // Time.timeScale = 0f;
    }

    private void Update()
    {
        // 게임 오버 상태에서 R키를 누르면 현재 씬 재시작
        if (IsGameOver && Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
    }

    private void RestartGame()
    {
        // 시간 흐름 복구
        Time.timeScale = 1f;
        // 현재 활성화된 씬 다시 로드
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
