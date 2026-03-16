using UnityEngine;

/// <summary>
/// 화면 상단에서 일정한 시간 간격으로 적을 랜덤하게 소환합니다.
/// 시간이 지남에 따라 생성 속도가 빨라지고, 한 번에 소환되는 적의 수가 늘어나는 난이도 상승 로직이 포함되어 있습니다.
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings (생성 속도 설정)")]
    [SerializeField] private GameObject _enemyPrefab;
    
    [Tooltip("게임 시작 시의 초기 적 생성 간격 (초)")]
    [SerializeField] private float _initialSpawnInterval = 1.5f;
    
    [Tooltip("적 생성 간격이 도달할 수 있는 최소값 (초)")]
    [SerializeField] private float _minSpawnInterval = 0.5f;
    
    [Tooltip("초당 생성 간격이 감소하는 비율 (값이 클수록 난이도가 빨리 상승함)")]
    [SerializeField] private float _difficultyScalingRate = 0.01f;

    [Header("Batch Settings (생성 마리수 설정)")]
    [Tooltip("게임 시작 시 한 번에 소환되는 적의 수")]
    [SerializeField] private int _initialEnemiesPerSpawn = 1;
    
    [Tooltip("몇 초마다 한 번에 소환되는 적의 수를 1마리씩 늘릴지 결정하는 간격")]
    [SerializeField] private float _batchIncreaseInterval = 30f;

    private float _currentSpawnInterval;
    private float _timer;
    private float _gameTime;
    private float _screenHalfWidth;
    private Camera _mainCam;

    private void Start()
    {
        _currentSpawnInterval = _initialSpawnInterval;
        _mainCam = Camera.main;
        if (_mainCam == null)
        {
            Debug.LogError("메인 카메라를 찾을 수 없습니다! Tag가 'MainCamera'로 설정되어 있는지 확인하세요.");
            return;
        }
        _screenHalfWidth = _mainCam.orthographicSize * _mainCam.aspect;
    }

    private void Update()
    {
        if (_mainCam == null) return;

        if (GameManager.Instance != null && GameManager.Instance.IsGameOver)
        {
            return;
        }

        _gameTime += Time.deltaTime;

        // 생성 속도 점진적 증가 (간격 감소)
        _currentSpawnInterval = Mathf.Max(_minSpawnInterval, _initialSpawnInterval - (_gameTime * _difficultyScalingRate));

        _timer += Time.deltaTime;
        if (_timer >= _currentSpawnInterval)
        {
            _timer = 0f;
            SpawnBatch();
        }
    }

    private void SpawnBatch()
    {
        // 시간에 따라 한 번에 소환하는 적의 수 계산 (30초마다 1마리씩 증가)
        int spawnCount = _initialEnemiesPerSpawn + Mathf.FloorToInt(_gameTime / _batchIncreaseInterval);
        
        for (int i = 0; i < spawnCount; i++)
        {
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        if (_enemyPrefab == null) return;

        float randomX = Random.Range(-_screenHalfWidth + 0.5f, _screenHalfWidth - 0.5f);
        Vector3 spawnPos = new Vector3(randomX, 6f, 0f);

        Instantiate(_enemyPrefab, spawnPos, Quaternion.identity);
    }
}
