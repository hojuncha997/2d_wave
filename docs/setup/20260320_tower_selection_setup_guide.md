# 타워 선택 및 데이터 기반 건설 시스템 설정 가이드 (전문가용 v2.1)

이 문서는 `TowerData`와 전용 UI 컴포넌트(`TowerSelectionButton`)를 활용하여, 유니티의 직렬화 버그를 방지하고 확장성이 뛰어난 타워 건설 시스템을 구축하는 정석적인 방법을 설명합니다.

---

## 🏗️ 1. 시스템 아키텍처 (전문가용 설계)
단순한 `OnClick` 연결 대신, 데이터와 로직을 분리한 **컴포넌트 기반 설계**를 사용합니다.

1.  **데이터 (`TowerData`)**: 타워의 스펙(가격, 프리팹 등)을 담은 스크립터블 오브젝트.
2.  **UI 로직 (`TowerSelectionButton`)**: 각 버튼에 붙어 자신이 어떤 타워를 담당하는지 알고 있으며, 클릭 시 이를 시스템에 전달하는 중개자.
3.  **시스템 관리 (`UIManager`)**: 현재 선택된 타워 상태를 유지하고 전체 UI를 통제.
4.  **건설 실행 (`TowerSlot`)**: 클릭 시 관리자에게 선택된 데이터를 물어보고 건설을 수행.

---

## 📂 2. 타워 데이터(TowerData) 에셋 설정
1.  `Assets/Data/Towers` 폴더 내에 **Create -> WaveShooter -> Tower Data**로 에셋 생성.
2.  **BasicTowerData**: 가격 `50`, 프리팹 `BasicTower`, 아이콘 할당.
3.  **ExplosiveTowerData**: 가격 `150`, 프리팹 `BomberTower`, 아이콘 할당.

---

## 🔘 3. 타워 선택 버튼(Button) 제작 (정석 방식)
유니티의 `UnityEvent` 직렬화 에러를 방지하기 위해 전용 컴포넌트를 사용합니다.

1.  **버튼 생성**: `TowerSelectionBar` 아래에 UI 버튼을 만들고 이름을 `Btn_BasicTower`로 변경.
2.  **전용 스크립트 추가**: `Btn_BasicTower` 오브젝트에 **`TowerSelectionButton`** 컴포넌트를 추가합니다.
3.  **데이터 할당**: 추가된 컴포넌트의 **`Tower Data`** 칸에 생성해둔 `BasicTowerData` 에셋을 드래그해서 넣습니다.
4.  **이벤트 연결 (중요)**:
    -   `Button` 컴포넌트의 `On Click()` 목록에서 **`+`** 클릭.
    -   **Object**: Hierarchy에 있는 **`Btn_BasicTower` (자기 자신)**을 드래그해서 넣습니다.
    -   **Function**: `TowerSelectionButton` -> **`SelectThisTower`**를 선택합니다.
    -   *참고: 인자가 없는 함수를 선택함으로써 유니티의 타입 변환 에러를 원천 차단합니다.*

---

## 🏗️ 4. 타워 슬롯(TowerSlot) 및 검증
1.  맵에 `TowerSlot` 프리팹 배치.
2.  **테스트**: 
    -   게임을 실행하고 하단 버튼 클릭 시 Console 창에 `[UIManager] 타워 선택됨: Basic Tower`가 뜨는지 확인.
    -   슬롯 클릭 시 골드가 차감되며 타워가 생성되는지 확인.

---

## 🛠️ 5. 문제 해결 (Troubleshooting)

### ❌ ArgumentException: Object of type 'UnityEngine.Object' cannot be converted...
*   **원인**: `UnityEvent`가 인스펙터에 할당된 커스텀 클래스(TowerData)를 런타임에 제대로 형변환하지 못해 발생하는 유니티 고질적 버그입니다.
*   **해결책 (정석)**: 본 가이드의 **3번 항목**처럼 별도의 `TowerSelectionButton` 컴포넌트를 만들어, 코드 상에서 직접 타입을 다루도록 우회합니다. (인스펙터의 `OnClick`에는 인자가 없는 함수를 연결합니다.)

### ❌ 버튼을 눌러도 반응이 없습니다.
*   `TowerSelectionButton` 컴포넌트에 `Tower Data` 에셋이 비어있는지 확인하세요.
*   `UIManager` 오브젝트가 씬에 배치되어 있고 `Instance`가 정상적으로 작동하는지 확인하세요.

### ❌ 슬롯 클릭 시 타워가 생기지 않습니다.
*   `GameManager`의 현재 골드가 타워 가격보다 적은지 확인하세요.
*   슬롯의 `Is Occupied`가 체크되어 있다면 이미 타워가 있는 것으로 간주됩니다.

---

## 💡 시각적 피드백 강화 (UX 팁)
*   **버튼 강조**: 버튼의 `Transition` 설정을 `Color Tint`로 하고, `Selected Color`를 밝은 노란색으로 설정하면 현재 어떤 타워가 선택되었는지 직관적으로 알 수 있습니다.
*   **툴팁 추가**: `TowerSelectionButton`에 `OnPointerEnter` 인터페이스를 구현하여, 마우스를 올렸을 때 타워 설명을 띄우도록 확장할 수 있습니다.
