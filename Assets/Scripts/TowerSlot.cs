using UnityEngine;

/// <summary>
/// 타워가 설치될 수 있는 개별 슬롯을 관리합니다.
/// </summary>
public class TowerSlot : MonoBehaviour
{
    [SerializeField] private GameObject _currentTowerPrefab;
    [SerializeField] private int _towerPrice = 50;
    [SerializeField] private bool _isOccupied = false;

    private GameObject _spawnedTower;

    private void Start()
    {
        // 만약 미리 설정된 타워가 있다면 설치 (게임 시작 시 기본 배치)
        if (_currentTowerPrefab != null && !_isOccupied)
        {
            DeployTower(_currentTowerPrefab);
        }
    }

    /// <summary>
    /// 마우스로 슬롯을 클릭했을 때 호출됩니다.
    /// </summary>
    private void OnMouseDown()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver) return;

        if (!_isOccupied)
        {
            // 타워 설치 로직
            if (GameManager.Instance != null && GameManager.Instance.UseGold(_towerPrice))
            {
                DeployTower(_currentTowerPrefab);
                Debug.Log($"타워 구매 완료! 소비된 골드: {_towerPrice}");
            }
        }
        else if (_spawnedTower != null)
        {
            // 타워 업그레이드 로직
            Tower tower = _spawnedTower.GetComponent<Tower>();
            if (tower != null)
            {
                if (GameManager.Instance != null && GameManager.Instance.UseGold(tower.UpgradeCost))
                {
                    tower.Upgrade();
                }
            }
        }
    }

    /// <summary>
    /// 슬롯에 타워를 배치합니다.
    /// </summary>
    public void DeployTower(GameObject towerPrefab)
    {
        if (_isOccupied) return;

        if (towerPrefab == null) return;

        // 슬롯의 위치(transform.position)에 타워 생성
        _spawnedTower = Instantiate(towerPrefab, transform.position, Quaternion.identity);
        _spawnedTower.transform.SetParent(this.transform); // 슬롯의 자식으로 설정
        _isOccupied = true;
    }

    // 에디터에서 슬롯의 위치를 사각형으로 시각화 (Gizmos)
    private void OnDrawGizmos()
    {
        Gizmos.color = _isOccupied ? Color.green : Color.white;
        // 1x1 크기의 사각형 와이어프레임 표시
        Gizmos.DrawWireCube(transform.position, new Vector3(1, 1, 0));
    }
}
