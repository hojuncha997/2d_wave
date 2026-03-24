using UnityEngine;

/// <summary>
/// 개별 타워 선택 버튼의 로직을 담당하는 컴포넌트입니다. (정석적인 UI 이벤트 분리 방식)
/// </summary>
public class TowerSelectionButton : MonoBehaviour
{
    [Header("Tower Settings")]
    [SerializeField] private TowerData _towerData; // 이 버튼이 클릭되었을 때 전달할 데이터

    /// <summary>
    /// 버튼이 클릭되었을 때 호출됩니다. (버튼의 OnClick 이벤트에 연결)
    /// </summary>
    public void SelectThisTower()
    {
        if (_towerData == null)
        {
            Debug.LogWarning($"[{name}] TowerData가 할당되지 않았습니다!");
            return;
        }

        if (UIManager.Instance != null)
        {
            // UIManager의 SelectTower 메서드를 직접 호출 (타입 안전성 확보)
            UIManager.Instance.SelectTower(_towerData);
        }
        else
        {
            Debug.LogError("[TowerSelectionButton] UIManager Instance를 찾을 수 없습니다!");
        }
    }
}
