using UnityEngine;

/// <summary>
/// 타워가 설치될 수 있는 개별 슬롯을 관리합니다.
/// </summary>
public class TowerSlot : MonoBehaviour
{
    [SerializeField] private GameObject _currentTowerPrefab;
    [SerializeField] private bool _isOccupied = false;

    private GameObject _spawnedTower;

    private void Start()
    {
        // 만약 미리 설정된 타워가 있다면 설치
        if (_currentTowerPrefab != null)
        {
            DeployTower(_currentTowerPrefab);
        }
    }

    /// <summary>
    /// 슬롯에 타워를 배치합니다.
    /// </summary>
    public void DeployTower(GameObject towerPrefab)
    {
        if (_isOccupied) return;

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
