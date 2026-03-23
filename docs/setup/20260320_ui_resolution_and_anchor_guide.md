# UI 해상도 대응 및 앵커(Anchor) 설정 가이드 (2026-03-20)

이 문서는 작업 환경(사무실/집)에 따라 모니터 해상도와 화면 비율이 달라질 때, UI가 화면 밖으로 사라지거나 위치가 어긋나는 문제를 해결하기 위한 설정법을 다룹니다.

---

## 1. 문제 원인 (Problem Diagnosis)
- **증상**: 씬(Scene) 뷰에서는 UI가 보이지만, 게임 실행 시(Play) 화면 밖으로 사라짐.
- **원인**: UI의 위치가 화면 중앙(Center)을 기준으로 고정된 픽셀 값(예: Y = -699)으로 설정되어 있어, 세로 해상도가 낮은 모니터에서는 화면 밑으로 밀려나게 됨.

---

## 2. 해결 방법 A: 앵커(Anchor) 최적화
UI 요소가 화면의 어느 모서리를 기준으로 붙어 있을지 결정하는 가장 중요한 설정입니다.

### [1] 기지 체력바 (HP Bar) 설정
1. Hierarchy에서 `BaseHPBar` 오브젝트 선택.
2. **RectTransform** -> **Anchor Presets** 아이콘 클릭.
3. **`Alt` 키를 누른 상태에서** **`Bottom Center`** (하단 중앙) 선택.
   - `Alt`를 누르면 앵커뿐만 아니라 실제 위치(Position)도 해당 위치로 이동합니다.
4. 이후 `Pos Y` 값을 `100` 정도로 설정하여 화면 하단에서 살짝 띄워줍니다.

### [2] 다른 UI 요소 권장 앵커
- **점수(Score) / 웨이브(Wave)**: `Top Center` 또는 `Top Left`.
- **골드(Gold)**: `Top Right`.
- **게임 오버 팝업**: `Middle Center`.

---

## 3. 해결 방법 B: Canvas Scaler 설정 (기기 대응)
모든 모니터에서 UI 크기를 비율에 맞게 자동으로 조절해 주는 설정입니다.

1. Hierarchy에서 **`Canvas`** 오브젝트 선택.
2. **Canvas Scaler** 컴포넌트 설정 변경:
   - **UI Scale Mode**: `Scale With Screen Size` (기본값인 Constant Pixel Size는 위험함)
   - **Reference Resolution**: `1920 x 1080` (기준이 될 해상도 입력)
   - **Screen Match Mode**: `Match Width Or Height`
   - **Match**: `0.5` (가로/세로 비율을 절반씩 반영)

---

## 4. 해결 방법 C: Game 뷰 고정 (작업 환경 통일)
유니티 에디터 상단 **Game 뷰** 탭에서 해상도 설정을 관리합니다.

- **권장 설정**: `Free Aspect` 대신 **`16:9`** 또는 **`1920x1080`**으로 고정.
- 이렇게 설정하면 어떤 모니터에서 작업하더라도 항상 동일한 화면 비율을 보며 작업할 수 있습니다.

---

## 💡 요약 체크리스트
- [ ] Canvas Scaler가 `Scale With Screen Size`인가?
- [ ] UI 요소들이 화면 중앙이 아닌 **가까운 모서리**에 앵커링 되어 있는가?
- [ ] Game 뷰 해상도가 `16:9` 등으로 고정되어 있는가?
