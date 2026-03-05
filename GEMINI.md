# GEMINI.md - 2D Wave Shooter Project Guidelines

이 파일은 Gemini CLI가 `2d_wave` 프로젝트를 지원할 때 준수해야 하는 핵심 지침과 프로젝트 정보를 담고 있습니다.

## 1. 프로젝트 개요
- **목적**: 모바일 및 PC 환경을 지원하는 2D 종스크롤 웨이브 슈팅 게임 개발.
- **엔진**: Unity (URP 기반 예상).
- **핵심 메커니즘**: 플레이어 조작(터치/키보드), 자동 발사, 적 생성 및 처치.

## 2. 코딩 컨벤션 (C#)
- **명명 규칙**:
    - 클래스/메서드: `PascalCase`
    - 변수: `camelCase`
    - private/protected 필드: `_` 접두사 사용 (예: `private float _moveSpeed;`)
    - [SerializeField] 필드: 인스펙터 노출을 위해 적극 활용하되, `private`으로 선언.
- **구조**:
    - 모든 스크립트는 적절한 네임스페이스(예: `WaveShooter.Core`)를 고려할 것. (현재는 기본값 유지 중)
    - `Update` 메서드 내에서의 `GetComponent` 호출은 금지. `Awake`나 `Start`에서 캐싱하여 사용.
- **최적화**:
    - 모바일 환경을 고려하여 가비지 컬렉션(GC) 발생을 최소화 (예: `Update`에서 매번 새로운 객체 생성 지양).
    - 추후 총알 및 적 생성 시 `Object Pooling` 기법 도입 고려.

## 3. 프로젝트 구조 규칙
- **Scripts**: `Assets/Scripts/` 폴더 내에 기능별로 분류.
- **Prefabs**: 모든 재사용 가능한 객체는 `Assets/Prefabs/`에 저장.
- **Scenes**: 메인 게임 씬은 `Assets/Scenes/`에 위치.

## 4. Gemini CLI 전용 지침
- 새로운 스크립트를 생성할 때 반드시 기존 `PlayerController`나 `Bullet`의 스타일을 참고하여 일관성을 유지할 것.
- UI 로직과 게임 로직을 가급적 분리하여 설계할 것.
- 기능을 추가할 때는 `Summary` 주석을 추가하여 코드의 의도를 명확히 할 것.

## 5. 문서 관리 규칙
- **파일명 규칙**: 모든 새 문서는 `YYYYMMDD_파일명.md` 형식을 사용한다. (예: `20260303_setup_guide.md`)
- **폴더 구조**: `docs/` 내부에 성격에 맞는 하위 폴더를 생성하여 관리한다.
    - `docs/setup/`: 유니티 에디터 설정 및 기술적 가이드
    - `docs/methodology/`: 개발 전략, 방법론, 워크플로우 가이드
    - `docs/ref/`: 참고 자료 및 규격 정보
