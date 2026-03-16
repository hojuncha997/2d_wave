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
    [SerializeField] private TextMeshProUGUI _goldText; // 추가: 골드 UI
    [SerializeField] private GameObject _gameOverUI;
    [SerializeField] private TextMeshProUGUI _finalScoreText;

    public bool IsGameOver { get; private set; } = false;
    public int Score { get; private set; } = 0;
    public int Gold { get; private set; } = 100; // 추가: 초기 자금 100G

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
        UpdateGoldUI(); // 추가: 초기 골드 UI 업데이트
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

    /// <summary>
    /// 골드를 추가하고 UI를 업데이트합니다.
    /// </summary>
    public void AddGold(int amount)
    {
        if (IsGameOver) return;

        Gold += amount;
        UpdateGoldUI();
        Debug.Log($"골드 획득! 현재 골드: {Gold}");
    }

    /// <summary>
    /// 골드를 소비합니다. 충분한 골드가 있으면 true를 반환하고 차감합니다.
    /// </summary>
    public bool UseGold(int amount)
    {
        if (Gold >= amount)
        {
            Gold -= amount;
            UpdateGoldUI();
            return true;
        }
        
        Debug.LogWarning($"골드가 부족합니다! (필요: {amount}, 보유: {Gold})");
        return false;
    }

    private void UpdateScoreUI()
    {
        if (_scoreText != null)
        {
            _scoreText.text = $"Score: {Score}";
        }
    }

    private void UpdateGoldUI()
    {
        if (_goldText != null)
        {
            _goldText.text = $"Gold: {Gold}G";
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
