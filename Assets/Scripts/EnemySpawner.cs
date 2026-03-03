using UnityEngine;

/// <summary>
/// 화면 상단에서 일정한 시간 간격으로 적을 랜덤하게 소환합니다.
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private float _spawnInterval = 1.5f;

    private float _timer;
    private float _screenHalfWidth;
    private Camera _mainCam;

    private void Start()
    {
        _mainCam = Camera.main;
        if (_mainCam == null)
        {
            Debug.LogError("메인 카메라를 찾을 수 없습니다! Tag가 'MainCamera'로 설정되어 있는지 확인하세요.");
            return;
        }
        // 화면 가장자리 너비 계산
        _screenHalfWidth = _mainCam.orthographicSize * _mainCam.aspect;
        Debug.Log($"EnemySpawner 시작됨: 화면 너비 절반 = {_screenHalfWidth}");
    }

    private void Update()
    {
        if (_mainCam == null) return;

        _timer += Time.deltaTime;
        if (_timer >= _spawnInterval)
        {
            _timer = 0f;
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        if (_enemyPrefab == null)
        {
            Debug.LogWarning("Enemy Prefab이 할당되지 않았습니다!");
            return;
        }

        float randomX = Random.Range(-_screenHalfWidth + 0.5f, _screenHalfWidth - 0.5f);
        Vector3 spawnPos = new Vector3(randomX, 6f, 0f); // y=6은 화면 위쪽

        Instantiate(_enemyPrefab, spawnPos, Quaternion.identity);
        Debug.Log($"적 소환됨! 위치: {spawnPos}");
    }
}
