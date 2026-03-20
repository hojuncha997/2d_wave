# 2026-03-20 타워 선택 시스템 구현 계획 (인수인계)

이 문서는 `feature/ui-integration` 브랜치에서 진행 중인 UI 통합 작업의 다음 단계인 **"타워 선택 및 건설 시스템"**에 대한 가이드입니다.

---

## 1. 현재 진행 상황 (Current Status)
- [x] **BaseManager**: 기지 체력 속성 및 변경 이벤트(`OnHealthChanged`) 구현 완료.
- [x] **UIManager**: HUD(Gold, Score, HP Bar) 통합 관리자 및 실시간 업데이트 로직 완료.
- [x] **GameManager**: UI 관리 로직을 UIManager로 이관(리팩토링) 완료.
- [x] **문서화**: UIManager 에디터 설정 가이드 작성 완료.

---

## 2. 다음 작업 목록 (Next To-Do)

### [1] TowerData (ScriptableObject) 생성
다양한 타워(기본, 폭발형 등)를 데이터로 관리하기 위한 설계도입니다.
- **필드**: `string towerName`, `int price`, `GameObject prefab`, `Sprite icon`.
- **목적**: 새로운 타워를 추가할 때 코드 수정 없이 에디터에서 파일만 만들면 되게끔 합니다.

### [2] UIManager - 타워 선택바(Tower Selection Bar) 구현
화면 하단에 플레이어가 타워를 고를 수 있는 UI를 추가합니다.
- **UI 구조**: `Horizontal Layout Group`을 활용한 버튼 리스트.
- **로직**: 버튼 클릭 시 `UIManager`가 현재 "선택된 타워 데이터"를 기억하도록 합니다.

### [3] TowerSlot 로직 수정
- **기존**: 클릭 시 무조건 미리 설정된 타워 건설.
- **변경**: `UIManager.Instance.SelectedTower`가 있을 때만 해당 타워를 건설하고, 골드를 차감합니다.

---

## 3. 기술적 구현 팁

### TowerData 예시 코드 (ScriptableObject)
```csharp
[CreateAssetMenu(fileName = "NewTower", menuName = "Tower Defense/Tower Data")]
public class TowerData : ScriptableObject {
    public string towerName;
    public int price;
    public GameObject prefab;
    public Sprite icon;
}
```

### 타워 배치 흐름
1. 플레이어가 하단 **'폭발형 타워' 버튼** 클릭.
2. `UIManager`의 `SelectedTower` 변수에 해당 데이터 저장.
3. 플레이어가 맵의 **`TowerSlot`** 클릭.
4. `TowerSlot`이 `UIManager`에게 물어봄: "지금 선택된 타워가 뭐야? 가격은 얼마야?"
5. 골드 충분하면 `GameManager.Instance.UseGold()` 호출 후 타워 생성.

---

## 4. 집에 가서 바로 할 일 (Quick Start)
1. 유니티 에디터에서 **`UIManager.cs`**에 `SelectedTower` 속성 추가하기.
2. 하단 UI 버튼을 만들고 클릭 시 데이터를 넘겨주는 간단한 스크립트 작성하기.
3. **`TowerSlot.cs`**의 `OnMouseDown` 부분을 수정하여 선택된 타워를 짓게 하기.
