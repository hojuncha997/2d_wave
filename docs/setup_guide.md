# Unity 프로젝트 설정 가이드 (2D Wave Shooter)

이 문서는 프로젝트의 핵심 시스템을 유니티 에디터에서 설정하는 방법을 설명합니다.

## 1. 적(Enemy) 시스템 설정
적이 화면 상단에서 자동으로 생성되어 내려오게 하려면 다음 단계를 따르세요.

### A. 적(Enemy) 프리팹 만들기
1. **오브젝트 생성**: Hierarchy 창 우클릭 -> 2D Object -> Sprites -> Square (사각형 생성).
2. **이름 변경**: 생성된 오브젝트 이름을 `Enemy`로 변경.
3. **컴포넌트 추가** (Inspector 창에서):
   - `Enemy.cs` 스크립트 추가.
   - `Box Collider 2D` 추가 후 **Is Trigger** 체크 (충돌 감지용).
4. **프리팹화**: Hierarchy의 `Enemy`를 프로젝트 창의 `Assets/Prefabs` 폴더로 드래그 앤 드롭.
5. **정리**: 프리팹을 만들었다면 Hierarchy에 있는 `Enemy`는 **삭제**합니다. (공장에서 찍어낼 '설계도'가 생겼으므로 원본은 지워도 됩니다.)

### B. 적 생성기(EnemySpawner) 설정
1. **공장 생성**: Hierarchy 창 우클릭 -> Create Empty (빈 오브젝트 생성).
2. **이름 변경**: 이름을 `EnemySpawner`로 변경.
3. **스크립트 추가**: `EnemySpawner.cs` 스크립트를 추가.
4. **설계도 연결**: Inspector 창의 `Enemy Prefab` 칸에 아까 만든 **Enemy 프리팹**을 드래그하여 할당합니다.
   - **주의**: 이 오브젝트는 Hierarchy에 **남겨두어야** 합니다. 게임 실행 중에 적을 계속 찍어내는 '공장' 역할을 하기 때문입니다.

## 2. 용어 정리
- **Hierarchy(하이어라키)**: 현재 씬(화면)에 나와 있는 실제 물체들.
- **Project(프로젝트)**: 게임에 쓰일 모든 파일(설계도, 이미지, 코드 등) 보관함.
- **Prefab(프리팹)**: 미리 만들어둔 오브젝트의 '설계도'. 언제든 복제해서 쓸 수 있음.
- **Is Trigger**: 물리적인 부딪힘(튕겨 나감) 없이 '겹쳐졌음'만 감지할 때 사용.

## 3. 실행 결과 확인
- 게임 실행(Play)을 누르면 Hierarchy 창에 `Enemy(Clone)`들이 자동으로 생겨나는지 확인하세요.
- 만약 안 보인다면 `EnemySpawner`의 `Enemy Prefab` 칸이 비어있는지(None) 확인하세요.

## 4. 충돌 처리(Collision) 설정 (나중에 할 일)
총알이 적을 맞췄을 때 적이 파괴되게 하려면 다음 두 가지를 설정해야 합니다.

### A. 적 프리팹에 태그(Tag) 설정
1. **프로젝트 창**에서 아까 만든 `Enemy` 프리팹을 선택합니다.
2. **Inspector 창** 맨 위쪽의 `Tag` 항목(기본값은 Untagged)을 클릭합니다.
3. 목록에서 **Enemy**를 선택합니다. (만약 목록에 없다면 `Add Tag...`를 눌러 `Enemy`라는 이름의 태그를 직접 추가한 후 다시 선택해야 합니다.)

### B. 총알(Bullet) 및 적(Enemy) 충돌체 설정
1. **Enemy 프리팹**: `Box Collider 2D`가 있고 **Is Trigger**가 체크되어 있어야 합니다.
2. **Bullet 프리팹**: `Box Collider 2D`가 있고 **Is Trigger**가 체크되어 있어야 합니다.
3. **Rigidbody 2D**: 충돌을 감지하기 위해 `Enemy`나 `Bullet` 중 하나에는 반드시 `Rigidbody 2D` 컴포넌트가 있어야 합니다. (보통 `Enemy`에 추가하고 `Gravity Scale`을 0으로 설정하여 아래로 떨어지지 않게 합니다.)

### C. 코드 적용 (Gemini에게 요청할 내용)
- "총알이 Enemy 태그를 가진 물체와 충돌하면 파괴되도록 Bullet.cs를 수정해줘."라고 요청하세요.
- 이 코드는 `OnTriggerEnter2D` 메서드를 사용하여 겹침을 감지하고, 상대방이 `Enemy` 태그를 가졌다면 `enemy.Die()`를 호출합니다.
