using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // TextMeshPro 기능을 사용하기 위해 필요

/// <summary>
/// 게임의 상태(진행, 종료, 재시작, 점수 및 UI)를 관리합니다.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private GameObject _gameOverUI;
    [SerializeField] private TextMeshProUGUI _finalScoreText;

    public bool IsGameOver { get; private set; } = false;
    public int Score { get; private set; } = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // 게임 시작 시 UI 초기화
        UpdateScoreUI();
        if (_gameOverUI != null) _gameOverUI.SetActive(false);
    }

    /// <summary>
    /// 점수를 추가하고 UI를 업데이트합니다.
    /// </summary>
    public void AddScore(int amount)
    {
        if (IsGameOver) return;

        Score += amount;
        UpdateScoreUI();
        Debug.Log($"현재 점수: {Score}");
    }

    private void UpdateScoreUI()
    {
        if (_scoreText != null)
        {
            _scoreText.text = $"Score: {Score}";
        }
    }

    /// <summary>
    /// 플레이어 사망 시 게임 오버 UI를 표시합니다.
    /// </summary>
    public void EndGame()
    {
        if (IsGameOver) return;

        IsGameOver = true;
        Debug.Log($"게임 오버! 최종 점수: {Score}. 'R'키를 눌러 재시작하세요.");

        // 게임 오버 UI 활성화
        if (_gameOverUI != null)
        {
            _gameOverUI.SetActive(true);
        }

        if (_finalScoreText != null)
        {
            _finalScoreText.text = $"Final Score: {Score}";
        }
    }

    private void Update()
    {
        if (IsGameOver && Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
    }

    private void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
