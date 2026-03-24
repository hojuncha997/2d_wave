using UnityEngine;

/// <summary>
/// 타워가 설치될 수 있는 개별 슬롯을 관리합니다. (v2.1 UI 연동 및 철거 기능 추가)
/// </summary>
public class TowerSlot : MonoBehaviour
{
    [Header("Initial Setup")]
    [SerializeField] private TowerData _initialTowerData; // 게임 시작 시 기본 배치할 타워 데이터
    [SerializeField] private bool _isOccupied = false;

    private GameObject _spawnedTower;

    private void Start()
    {
        // 만약 미리 설정된 타워 데이터가 있다면 설치 (게임 시작 시 기본 배치)
        if (_initialTowerData != null && !_isOccupied)
        {
            DeployTower(_initialTowerData);
        }
    }

    /// <summary>
    /// 마우스로 슬롯을 클릭했을 때 호출됩니다. (v2.0 UI 선택 연동)
    /// </summary>
    private void OnMouseDown()
    {
        // 1. 이미 타워가 있거나 게임 오버라면 무시
        if (_isOccupied || (GameManager.Instance != null && GameManager.Instance.IsGameOver)) return;

        // 2. UIManager에서 현재 선택된 타워 데이터 가져오기
        TowerData selectedTower = UIManager.Instance != null ? UIManager.Instance.SelectedTower : null;

        if (selectedTower == null)
        {
            Debug.Log("[TowerSlot] 선택된 타워가 없습니다. 하단 UI에서 타워를 먼저 선택하세요.");
            return;
        }

        // 3. 골드 체크 및 소비
        if (GameManager.Instance != null && GameManager.Instance.UseGold(selectedTower.price))
        {
            DeployTower(selectedTower);
            Debug.Log($"[{selectedTower.towerName}] 구매 완료! 소비된 골드: {selectedTower.price}");
        }
    }

    /// <summary>
    /// 슬롯 위에 마우스가 있을 때 매 프레임 호출됩니다. (v2.1 디버그/철거용 추가)
    /// </summary>
    private void OnMouseOver()
    {
        // 우클릭 시 타워 철거 (테스트 및 기능용)
        if (Input.GetMouseButtonDown(1) && _isOccupied)
        {
            RemoveTower();
        }
    }

    /// <summary>
    /// 슬롯에 타워를 배치합니다. (TowerData 기반)
    /// </summary>
    public void DeployTower(TowerData data)
    {
        if (_isOccupied || data == null || data.towerPrefab == null) return;

        // 슬롯의 위치(transform.position)에 타워 생성
        _spawnedTower = Instantiate(data.towerPrefab, transform.position, Quaternion.identity);
        _spawnedTower.transform.SetParent(this.transform); // 슬롯의 자식으로 설정
        _isOccupied = true;
    }

    /// <summary>
    /// 슬롯에서 타워를 제거(철거)합니다. (v2.1 추가)
    /// </summary>
    public void RemoveTower()
    {
        if (_spawnedTower != null)
        {
            Destroy(_spawnedTower);
            _spawnedTower = null;
        }
        _isOccupied = false;
        Debug.Log($"[{name}] 타워가 철거되었습니다.");
    }

    // 에디터에서 슬롯의 위치를 사각형으로 시각화 (Gizmos)
    private void OnDrawGizmos()
    {
        Gizmos.color = _isOccupied ? Color.green : Color.white;
        // 1x1 크기의 사각형 와이어프레임 표시
        Gizmos.DrawWireCube(transform.position, new Vector3(1, 1, 0));
    }
}
