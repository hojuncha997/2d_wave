using UnityEngine;

/// <summary>
/// 이동 및 직접 사격 로직이 제거된 플레이어 컨트롤러입니다.
/// 현재 고정형 타워 디펜스 슈팅으로 피벗됨에 따라, 
/// 체력 관리와 적 충돌 처리는 BaseManager가 전담합니다.
/// 추후 타워 배치, 업그레이드 선택 등 UI 상호작용 및 플레이어 입력 처리에 재활용될 수 있습니다.
/// </summary>
public class PlayerController : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("PlayerController: 이동 및 사격 로직이 제거되었습니다. 시스템 역할은 BaseManager로 이관되었습니다.");
    }
}
