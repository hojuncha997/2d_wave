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
    [SerializeField] private TextMeshProUGUI _announcementText;

    private int _currentWaveIndex = 0;
    private bool _isWaveInProgress = false;
    private bool _isReadyForNextWave = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        // 웨이브가 진행 중이 아닐 때만 입력을 받아 다음 웨이브를 시작함
        if (!_isWaveInProgress && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            _isReadyForNextWave = true;
        }
    }

    private void Start()
    {
        if (_spawner == null) _spawner = FindFirstObjectByType<EnemySpawner>();
        
        if (_waves.Count == 0)
        {
            Debug.LogWarning("WaveManager: 등록된 웨이브 데이터가 없습니다!");
        }

        StartCoroutine(GameLoopRoutine());
    }

    private IEnumerator GameLoopRoutine()
    {
        yield return new WaitForSeconds(1.5f);

        while (_currentWaveIndex < _waves.Count)
        {
            WaveData currentWave = _waves[_currentWaveIndex];
            _isReadyForNextWave = false;

            // 준비 메시지 설정
            string prepareMessage = (_currentWaveIndex == 0) 
                ? $"<size=40>{currentWave.WaveName}</size>\nClick or Space to START!"
                : $"<color=green>WAVE CLEAR!</color>\nUpgrade Towers and Start \n<size=35>{currentWave.WaveName}</size>";

            yield return ShowAnnouncement(prepareMessage, 0f);
            
            // 사용자가 클릭할 때까지 무한 대기
            yield return new WaitUntil(() => _isReadyForNextWave);

            // 실제 웨이브 실행 및 종료까지 대기
            yield return StartCoroutine(PlayWave(currentWave));
            
            _currentWaveIndex++;
        }

        yield return ShowAnnouncement("<color=yellow>★ MISSION COMPLETE ★</color>\nBase Defended!", 5f);
    }

    private IEnumerator PlayWave(WaveData data)
    {
        _isWaveInProgress = true;
        UpdateWaveUI(_currentWaveIndex + 1);

        // 웨이브 시작 문구
        string startMsg = (data.WaveName.Contains("Boss")) 
            ? $"<color=red>WARNING: {data.WaveName}!</color>" 
            : $"NOW START: {data.WaveName} (Wave {_currentWaveIndex + 1})";
            
        yield return ShowAnnouncement(startMsg, 1.5f);

        if (_spawner != null)
        {
            _spawner.StartWave(data);
        }
        else
        {
            Debug.LogError("WaveManager: Spawner missing!");
            _isWaveInProgress = false;
            yield break;
        }

        // 소환 시작 대기
        yield return null;

        // 모든 적이 처치될 때까지 대기
        yield return new WaitUntil(() => IsWaveCleared());

        Debug.Log($"Wave {_currentWaveIndex + 1} Clear!");
        _isWaveInProgress = false;
    }

    private bool IsWaveCleared()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver) return true;
        if (_spawner != null && _spawner.IsSpawning) return false;
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
            
            if (duration > 0f)
            {
                yield return new WaitForSeconds(duration);
                _announcementText.gameObject.SetActive(false);
            }
        }
        else
        {
            Debug.Log($"[Announcement] {message}");
            if (duration > 0f) yield return new WaitForSeconds(duration);
        }
    }
}
