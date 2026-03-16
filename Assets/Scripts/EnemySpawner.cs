using UnityEngine;

/// <summary>
/// 화면 상단에서 일정한 시간 간격으로 적을 랜덤하게 소환합니다.
/// 시간이 지남에 따라 생성 속도가 빨라지고, 한 번에 소환되는 적의 수가 늘어나는 난이도 상승 로직이 포함되어 있습니다.
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    [Header("Mode Settings")]
    [Tooltip("체크하면 기존의 무한 소환 로직을 사용하고, 체크 해제하면 WaveManager의 제어를 받습니다.")]
    [SerializeField] private bool _useInfiniteMode = false;

    [Header("Infinite Mode Settings (기존 로직)")]
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private float _initialSpawnInterval = 1.5f;
    [SerializeField] private float _minSpawnInterval = 0.5f;
    [SerializeField] private float _difficultyScalingRate = 0.01f;
    [SerializeField] private int _initialEnemiesPerSpawn = 1;
    [SerializeField] private float _batchIncreaseInterval = 30f;

    private float _currentSpawnInterval;
    private float _timer;
    private float _gameTime;
    private float _screenHalfWidth;
    private Camera _mainCam;
    private Coroutine _currentSpawnCoroutine;

    /// <summary>
    /// 현재 적을 소환 중인지 여부를 반환합니다.
    /// </summary>
    public bool IsSpawning => _currentSpawnCoroutine != null;

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
        if (_mainCam == null || !_useInfiniteMode) return;

        if (GameManager.Instance != null && GameManager.Instance.IsGameOver)
        {
            return;
        }

        _gameTime += Time.deltaTime;

        _currentSpawnInterval = Mathf.Max(_minSpawnInterval, _initialSpawnInterval - (_gameTime * _difficultyScalingRate));

        _timer += Time.deltaTime;
        if (_timer >= _currentSpawnInterval)
        {
            _timer = 0f;
            SpawnBatch();
        }
    }

    /// <summary>
    /// 외부(WaveManager)에서 호출하여 특정 웨이브 소환을 시작합니다.
    /// </summary>
    public void StartWave(WaveData waveData)
    {
        if (_currentSpawnCoroutine != null)
        {
            StopCoroutine(_currentSpawnCoroutine);
        }
        _currentSpawnCoroutine = StartCoroutine(SpawnWaveRoutine(waveData));
    }

    /// <summary>
    /// 웨이브 데이터를 바탕으로 적을 순차적으로 소환하는 코루틴입니다.
    /// </summary>
    private System.Collections.IEnumerator SpawnWaveRoutine(WaveData waveData)
    {
        Debug.Log($"웨이브 {waveData.WaveNumber} 소환 시작!");

        foreach (var batch in waveData.Batches)
        {
            for (int i = 0; i < batch.Count; i++)
            {
                // 게임 오버 시 소환 중단
                if (GameManager.Instance != null && GameManager.Instance.IsGameOver)
                    yield break;

                SpawnEnemyPrefab(batch.EnemyPrefab);
                yield return new WaitForSeconds(batch.SpawnInterval);
            }
        }

        Debug.Log($"웨이브 {waveData.WaveNumber} 모든 적 소환 완료.");
        _currentSpawnCoroutine = null;
    }

    private void SpawnBatch()
    {
        int spawnCount = _initialEnemiesPerSpawn + Mathf.FloorToInt(_gameTime / _batchIncreaseInterval);
        
        for (int i = 0; i < spawnCount; i++)
        {
            SpawnEnemyPrefab(_enemyPrefab);
        }
    }

    /// <summary>
    /// 실제 적 프리팹을 화면에 생성하는 공통 로직입니다.
    /// </summary>
    private void SpawnEnemyPrefab(GameObject prefab)
    {
        if (prefab == null) return;

        float randomX = Random.Range(-_screenHalfWidth + 0.5f, _screenHalfWidth - 0.5f);
        Vector3 spawnPos = new Vector3(randomX, 6f, 0f);

        Instantiate(prefab, spawnPos, Quaternion.identity);
    }
}
