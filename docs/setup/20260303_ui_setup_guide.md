# 유니티 UI(Score & Game Over) 마스터 가이드 (2026-03-03)

이 문서는 게임 화면에 UI를 배치하고 관리할 때 초보자가 겪을 수 있는 모든 상황과 해결책을 담고 있습니다.

## 1. UI 도화지 (Canvas) 기초 설정
1. **생성**: Hierarchy 우클릭 -> **UI** -> **Canvas** (이름: **`HUD`**).
2. **해상도**: `HUD` 클릭 -> Inspector의 **`Canvas Scaler`**에서:
   - `UI Scale Mode`: **Scale With Screen Size**.
   - `Reference Resolution`: **1080 x 1920**.

## 2. Scene 뷰에서 UI를 찾는 법 (매우 중요!)
플레이어(작은 물체)와 UI(거대한 도화지)의 크기 차이 때문에 UI가 안 보일 수 있습니다.
- **찾기**: Hierarchy 창에서 **`HUD`**나 **`ScoreText`**를 **더블클릭** 하세요. 카메라가 자동으로 UI 영역으로 점프합니다.
- **줌 아웃**: 마우스 휠을 아래로 끝까지 굴려보세요. 플레이어 옆에 거대한 흰색 사각형이 나타납니다. 그게 바로 Canvas입니다.

## 3. 텍스트 배치 및 스타일 (TextMeshPro)
1. **생성**: `HUD` 우클릭 -> **UI** -> **Text - TextMeshPro**.
   - 처음 생성 시 뜨는 팝업창에서 **"Import TMP Essentials"**를 꼭 눌러주세요.
2. **스타일** (Inspector 중간의 **`Text Mesh Pro`** 섹션):
   - **Text Input**: 표시할 글자 입력 (예: `Score: 0`).
   - **Font Size**: 글자 크기 조절 (50~80 권장).
   - **Vertex Color**: 글자 색상 변경.
3. **위치 잡기** (Inspector 맨 위의 **`Rect Transform`** 섹션):
   - **Pos Y**: 글자가 겹친다면 이 숫자를 직접 수정하세요. (예: `150`, `-150`)
   - **Width/Height**: 글자가 잘리면 이 값을 넉넉히 늘려주세요.

## 4. 게임 오버 화면 (GameOverUI) 관리
1. **구조**: `HUD` 아래에 **`GameOverUI` (Panel)**를 만들고, 그 자식으로 `GameOverText`와 `FinalScoreText`를 넣습니다.
2. **회색 필터 문제**: 패널을 만들면 화면이 어두워집니다. 평소에는 꺼두어야 합니다.
   - **비활성화**: Hierarchy의 **`GameOverUI`** 클릭 -> Inspector 맨 위 **체크박스 해제**. (이름이 옅은 회색으로 변하면 성공!)

## 5. 최종 연결: GameManager 설정
이 단계가 빠지면 코드가 UI를 조작하지 못합니다.
1. Hierarchy의 **`GameManager`** 클릭.
2. Inspector의 **`GameManager (Script)`** 섹션에 있는 빈 칸들에 드래그 앤 드롭:
   - `Score Text` -> Hierarchy의 **`ScoreText`**
   - `Game Over UI` -> Hierarchy의 **`GameOverUI`**
   - `Final Score Text` -> `GameOverUI` 자식인 **`FinalScoreText`**

## 6. 유니티 긴급 조작 팁
- **창이 닫혔을 때 (단축키)**:
  - **Scene 뷰**: `Ctrl + 1`
  - **Game 뷰**: `Ctrl + 2`
  - **Inspector**: `Ctrl + 3` (또는 `Alt + 3`)
- **도구가 안 보일 때**: 
  - 화면 왼쪽 위 **`Rect Tool` (단축키: `T`)**을 클릭하세요. UI를 옮기기에 가장 좋습니다.
  - 마우스로 옮기기 힘들면 Inspector에서 **`Pos X / Pos Y`** 숫자를 직접 수정하세요.
- **레이아웃 초기화**: 오른쪽 위 **`Layout`** 버튼 -> **`Default`** 선택.
