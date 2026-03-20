using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 게임의 모든 UI(HUD, 게임 오버 화면 등)를 통합 관리합니다.
/// </summary>
public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("HUD Elements")]
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _goldText;
    [SerializeField] private Slider _hpSlider;

    [Header("Game Over UI")]
    [SerializeField] private GameObject _gameOverUI;
    [SerializeField] private TextMeshProUGUI _finalScoreText;

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
        // 초기 UI 상태 설정
        if (_gameOverUI != null) _gameOverUI.SetActive(false);

        // BaseManager 이벤트 구독
        if (BaseManager.Instance != null)
        {
            BaseManager.Instance.OnHealthChanged += UpdateHealthBar;
            // 초기 체력바 설정
            UpdateHealthBar(BaseManager.Instance.CurrentHp, BaseManager.Instance.MaxHp);
        }
    }

    private void OnDestroy()
    {
        // 이벤트 구독 해제 (메모리 누수 방지)
        if (BaseManager.Instance != null)
        {
            BaseManager.Instance.OnHealthChanged -= UpdateHealthBar;
        }
    }

    /// <summary>
    /// 점수 텍스트를 업데이트합니다.
    /// </summary>
    public void UpdateScore(int score)
    {
        if (_scoreText != null)
        {
            _scoreText.text = $"Score: {score}";
        }
    }

    /// <summary>
    /// 골드 텍스트를 업데이트합니다.
    /// </summary>
    public void UpdateGold(int gold)
    {
        if (_goldText != null)
        {
            _goldText.text = $"Gold: {gold}G";
        }
    }

    /// <summary>
    /// 기지 체력바를 업데이트합니다.
    /// </summary>
    public void UpdateHealthBar(int currentHp, int maxHp)
    {
        if (_hpSlider != null)
        {
            _hpSlider.maxValue = maxHp;
            _hpSlider.value = currentHp;
        }
    }

    /// <summary>
    /// 게임 오버 UI를 활성화하고 최종 점수를 표시합니다.
    /// </summary>
    public void ShowGameOver(int finalScore)
    {
        if (_gameOverUI != null)
        {
            _gameOverUI.SetActive(true);
        }

        if (_finalScoreText != null)
        {
            _finalScoreText.text = $"Final Score: {finalScore}";
        }
    }
}
