using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

/// <summary>
/// 게임의 웨이브 흐름을 총괄 관리하는 매니저입니다.
/// </summary>
public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance { get; private set; }

    [Header("Wave Configuration")]
    [SerializeField] private List<WaveData> _waves = new List<WaveData>();
    [SerializeField] private EnemySpawner _spawner;
    
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI _waveText;
    [SerializeField] private TextMeshProUGUI _announcementText; // "Wave 1 Start!" 등을 표시할 큰 텍스트

    private int _currentWaveIndex = 0;
    private bool _isWaveInProgress = false;
    private int _remainingEnemiesInWave = 0;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        if (_spawner == null) _spawner = FindFirstObjectByType<EnemySpawner>();
        
        Debug.Log($"WaveManager 시작: 등록된 웨이브 수 = {_waves.Count}");
        
        if (_waves.Count == 0)
        {
            Debug.LogWarning("WaveManager: 등록된 웨이브 데이터가 없습니다! 인스펙터의 Waves 리스트를 확인하세요.");
        }

        // 첫 번째 웨이브 시작
        StartCoroutine(GameLoopRoutine());
    }

    /// <summary>
    /// 전체 게임 루프를 관리하는 코루틴입니다.
    /// </summary>
    private IEnumerator GameLoopRoutine()
    {
        yield return new WaitForSeconds(1f); // 시작 전 잠시 대기

        while (_currentWaveIndex < _waves.Count)
        {
            yield return StartCoroutine(PlayWave(_waves[_currentWaveIndex]));
            
            _currentWaveIndex++;
            
            if (_currentWaveIndex < _waves.Count)
            {
                // 다음 웨이브 전 대기 (Intermission)
                float waitTime = _waves[_currentWaveIndex - 1].WaitTimeAfterWave;
                yield return ShowAnnouncement($"Wave Clear! Next wave in {waitTime}s", waitTime);
            }
        }

        yield return ShowAnnouncement("All Waves Cleared! Victory!", 5f);
        // 여기서 승리 연출이나 게임 종료 로직 추가 가능
    }

    /// <summary>
    /// 단일 웨이브를 실행하고 종료될 때까지 대기합니다.
    /// </summary>
    private IEnumerator PlayWave(WaveData data)
    {
        _isWaveInProgress = true;
        UpdateWaveUI(data.WaveNumber);
        
        yield return ShowAnnouncement($"Wave {data.WaveNumber} Start!", 2f);

        // 스포너에게 소환 명령
        if (_spawner != null)
        {
            _spawner.StartWave(data);
        }
        else
        {
            Debug.LogError("WaveManager: EnemySpawner가 연결되지 않았습니다!");
            yield break;
        }

        // 소환이 시작될 때까지 한 프레임 대기
        yield return null;

        // 모든 적이 소환 완료되고, 화면의 모든 적이 처치될 때까지 대기
        yield return new WaitUntil(() => IsWaveCleared());

        Debug.Log($"웨이브 {data.WaveNumber} 클리어!");
        _isWaveInProgress = false;
    }

    private bool IsWaveCleared()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver) return true;

        // 1. 아직 소환 중이라면 클리어 아님
        if (_spawner != null && _spawner.IsSpawning) return false;

        // 2. 소환이 끝났고, 화면에 적이 하나도 없는지 체크
        return GameObject.FindGameObjectsWithTag("Enemy").Length == 0;
    }

    private void UpdateWaveUI(int waveNumber)
    {
        if (_waveText != null)
            _waveText.text = $"Wave: {waveNumber}";
    }

    private IEnumerator ShowAnnouncement(string message, float duration)
    {
        if (_announcementText != null)
        {
            _announcementText.text = message;
            _announcementText.gameObject.SetActive(true);
            yield return new WaitForSeconds(duration);
            _announcementText.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log($"[Announcement] {message}");
            yield return new WaitForSeconds(duration);
        }
    }
}
