# 발사체 상속 시스템 및 착탄 마커 가이드 (2026-03-17)

이 문서는 모든 탄약의 기반이 되는 상속 시스템과, 곡사포용 착탄 지점 시각화(Landing Marker)의 설정 방법을 설명합니다.

---

## 1. 시스템 구조 (Inheritance)

### [1] BaseProjectile (부모)
모든 탄약의 공통 이동 및 관리 로직을 담당합니다.
- **Movement Mode**: 직선(Straight) 또는 곡사(Arc) 중 선택 가능.
- **Homing Option**: 끝까지 추적할지, 초기 위치로 발사할지 결정.
- **Landing Marker**: 착탄 지점에 시각적 마커 소환 기능 내장.

### [2] 자식 클래스 (Bullet, ExplosiveBullet)
- **Bullet**: 단일 대상 타격 및 데미지 전달.
- **ExplosiveBullet**: 착탄 지점 주변 광역 데미지 및 곡사포 모드 지원.

---

## 2. 유니티 에디터: 착탄 마커(Landing Marker) 제작법

나중에 새로운 마커(예: 조준점, 마법진 등)를 만들 때 다음 단계를 참고하세요.

1. **오브젝트 생성**: Hierarchy > 우클릭 > `2D Object > Sprites > Circle` 생성.
2. **색상 및 투명도**: Sprite Renderer의 `Color` 클릭 후 `Alpha(A)` 값을 `50` 정도로 낮춤 (반투명 효과).
3. **레이어 순서**: Sprite Renderer의 `Order in Layer`를 `-1`로 설정 (바닥에 깔리게 함).
4. **프리팹화**: 만든 오브젝트를 Project 창으로 드래그하여 프리팹으로 저장.
5. **연결**: `ExplosiveBullet` 프리팹의 `Landing Marker Prefab` 슬롯에 위에서 만든 프리팹을 드래그하여 연결.

---

## 3. 핵심 이동 모드 설정 (Inspector)

| 옵션 항목 | 설정값 | 효과 |
| :--- | :--- | :--- |
| **Use Arc Movement** | 체크(V) | 포물선을 그리며 위로 솟구쳤다가 떨어짐. |
| **Arc Height** | 2 ~ 3 | 포물선의 높이 (클수록 높게 날아감). |
| **Is Homing** | 해제( ) | 적이 피할 수 있는 '무식한 대포' 모드. |
| **Is Homing** | 체크(V) | 적을 끝까지 쫓아가는 '유도 미사일' 모드. |

---

## 4. 고급 팁
- **폭발 반경 연동**: `ExplosiveBullet`은 인스펙터의 `Explosion Radius` 값에 따라 착탄 마커의 크기를 자동으로 조절합니다. (따로 마커 크기를 키울 필요가 없습니다.)
- **마커 이미지 변경**: 단순한 원형이 아닌, 'X'자 모양이나 목표지점 아이콘 이미지를 사용하고 싶다면 Sprite Renderer의 `Sprite` 항목만 교체하면 됩니다.
