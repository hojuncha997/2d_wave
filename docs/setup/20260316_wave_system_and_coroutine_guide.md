# 웨이브 시스템 구현 및 코루틴 가이드 (2026-03-16)

이 문서는 데이터 기반 웨이브 시스템의 구현 방식과 핵심 기술인 코루틴, 그리고 유니티 에디터 설정 방법을 상세히 설명합니다.

---

## 1. 시스템 아키텍처
본 웨이브 시스템은 **데이터(Data) - 제어(Control) - 실행(Execution)**의 3단계 구조로 설계되었습니다.

1. **WaveData (Data)**: `ScriptableObject`를 사용하여 적의 종류, 마리수, 생성 간격 등의 정보를 에셋 파일로 저장합니다.
2. **WaveManager (Control)**: 전체 게임의 흐름을 관리합니다. 웨이브 간 대기 시간을 처리하고, 모든 적이 전멸했는지 감시합니다.
3. **EnemySpawner (Execution)**: 매니저의 명령을 받아 실제 적 프리팹을 화면에 생성합니다.

---

## 2. 코루틴(Coroutine) 상세 설명

### [1] 코루틴이란?
일반적인 함수는 호출되면 실행을 마치고 즉시 반환(Return)되지만, **코루틴**은 실행을 일시 중단(yield)했다가 특정 조건이나 시간이 지난 후 중단된 지점부터 다시 실행할 수 있는 특수한 함수입니다.

### [2] 핵심 키워드
- `IEnumerator`: 코루틴 함수의 반환 타입입니다.
- `yield return new WaitForSeconds(n)`: n초 동안 실행을 멈추고 유니티에게 제어권을 넘깁니다.
- `yield return null`: 다음 프레임까지 대기합니다.
- `yield return new WaitUntil(조건)`: 특정 조건이 참이 될 때까지 대기합니다.
- `StartCoroutine()`: 코루틴을 실행하는 명령입니다.

### [3] 프로젝트 적용 사례
- **EnemySpawner**: `SpawnWaveRoutine` 코루틴을 사용하여 적을 한 마리 소환한 뒤 `SpawnInterval`만큼 정확히 쉬고 다음 적을 소환합니다. (Update문을 복잡하게 사용하지 않아도 됨)
- **WaveManager**: `GameLoopRoutine`을 통해 웨이브 시작 -> 진행 -> 클리어 체크 -> 인터미션 대기를 하나의 흐름으로 깔끔하게 제어합니다.

---

## 3. 유니티 에디터 UI 및 에셋 설정 가이드

### [1] WaveData 에셋 생성
1. **Project** 창에서 우클릭 -> **Create > Wave System > Wave Data**를 클릭합니다.
2. 생성된 에셋 파일의 이름을 `Wave_01` 등으로 변경합니다.
3. **Inspector** 창에서 다음 내용을 채웁니다:
    - `Wave Number`: 현재 웨이브 번호.
    - `Batches`: `+` 버튼을 눌러 적 프리팹, 마리수, 간격을 설정합니다.

### [2] UI (TextMeshPro) 설정
1. **Canvas** 오브젝트 하위에 두 개의 텍스트를 생성합니다:
    - **WaveText**: 화면 구석에 배치 (예: "Wave: 1" 표시).
    - **AnnouncementText**: 화면 중앙에 배치 (예: "Wave Start!" 알림).
2. `WaveManager` 오브젝트를 선택하고, 인스펙터의 해당 슬롯에 위 텍스트들을 드래그하여 연결합니다.

### [3] WaveManager 연결 (중요)
1. Hierarchy 창에서 **WaveManager** 오브젝트를 클릭합니다.
2. **Waves** 리스트의 `+` 버튼을 눌러 슬롯을 만듭니다.
3. [1]에서 만든 **Wave_01** 에셋 파일을 이 슬롯에 드래그하여 등록합니다.
    - **주의**: 이 리스트가 비어 있으면 게임 시작 시 바로 "Victory" 문구가 뜨며 종료됩니다.

---

## 4. 트러블슈팅
- **즉시 승리 문구가 뜨는 경우**: `WaveManager`의 `Waves` 리스트에 데이터가 등록되지 않았거나, `EnemySpawner`가 누락된 경우입니다. 인스펙터를 확인하세요.
- **적이 소환되지 않는 경우**: `EnemySpawner`의 `Use Infinite Mode` 체크가 해제되어 있는지 확인하고, `WaveData` 에셋에 적 프리팹이 할당되었는지 확인하세요.
