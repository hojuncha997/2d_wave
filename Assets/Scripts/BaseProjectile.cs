using UnityEngine;

/// <summary>
/// 모든 발사체의 공통 기능을 담당하는 베이스 클래스입니다.
/// 직선 이동 및 포물선(곡사) 이동을 지원하며, 자식 클래스에서 세부 효과를 구현합니다.
/// </summary>
public abstract class BaseProjectile : MonoBehaviour
{
    [Header("Base Projectile Settings")]
    [SerializeField] protected float _speed = 10f;
    [SerializeField] protected float _lifeTime = 3f;
    
    [Header("Movement Mode")]
    [Tooltip("체크하면 포물선(Arc) 궤적으로 이동합니다.")]
    [SerializeField] protected bool _useArcMovement = false;
    [Tooltip("포물선의 최대 높이입니다.")]
    [SerializeField] protected float _arcHeight = 2f;
    [Tooltip("체크하면 타겟을 끝까지 추적합니다. 해제하면 발사 시점의 위치로 날아갑니다.")]
    [SerializeField] protected bool _isHoming = true;

    [Header("Visual Aids")]
    [Tooltip("착탄 지점에 표시될 마커 프리팹입니다. (그림자나 조준점 등)")]
    [SerializeField] protected GameObject _landingMarkerPrefab;

    protected Transform _target;
    protected Vector3 _startPosition;
    protected Vector3 _targetPosition;
    protected float _progress = 0f;
    protected Vector3 _lastDirection = Vector3.up;
    protected GameObject _spawnedMarker;

    protected virtual void Start()
    {
        _startPosition = transform.position;
        // 발사 시점에 타겟이 있다면 초기 목표 위치 저장
        if (_target != null)
        {
            _targetPosition = _target.position;
            _lastDirection = (_targetPosition - _startPosition).normalized;
        }

        // 착탄 마커 생성
        SpawnLandingMarker();
        
        Destroy(gameObject, _lifeTime);
    }

    /// <summary>
    /// 목표 지점에 착탄 마커를 소환하고 배치합니다.
    /// </summary>
    protected virtual void SpawnLandingMarker()
    {
        if (_landingMarkerPrefab != null)
        {
            _spawnedMarker = Instantiate(_landingMarkerPrefab, _targetPosition, Quaternion.identity);
        }
    }

    protected virtual void OnDestroy()
    {
        // 발사체가 파괴될 때 마커도 함께 제거
        if (_spawnedMarker != null)
        {
            Destroy(_spawnedMarker);
        }
    }

    /// <summary>
    /// 발사체의 목표 타겟을 설정합니다.
    /// </summary>
    public virtual void SetTarget(Transform target)
    {
        _target = target;
        if (_target != null)
        {
            _targetPosition = _target.position;
        }
    }

    protected virtual void Update()
    {
        // 유도 모드인 경우 실시간으로 타겟 위치 및 방향 갱신
        if (_isHoming && _target != null)
        {
            _targetPosition = _target.position;
            _lastDirection = (_targetPosition - transform.position).normalized;
        }

        if (_useArcMovement)
        {
            MoveInArc();
        }
        else
        {
            MoveStraight();
        }
    }

    /// <summary>
    /// 타겟을 향한 직선 이동 (Update에서 갱신된 _lastDirection 사용)
    /// </summary>
    protected virtual void MoveStraight()
    {
        UpdateRotation(_lastDirection);
        transform.position += _lastDirection * _speed * Time.deltaTime;
    }

    /// <summary>
    /// 시작점부터 목표 지점까지 포물선을 그리며 이동
    /// </summary>
    protected virtual void MoveInArc()
    {
        // 거리와 속도에 따른 진행률(0~1) 계산
        float totalDistance = Vector3.Distance(_startPosition, _targetPosition);
        if (totalDistance > 0)
        {
            _progress += Time.deltaTime * (_speed / totalDistance);
        }
        else
        {
            _progress = 1f;
        }
        
        _progress = Mathf.Clamp01(_progress);

        // 선형 보간(Lerp)으로 평면 위치 계산
        Vector3 currentPos = Vector3.Lerp(_startPosition, _targetPosition, _progress);

        // Sine 곡선을 이용한 높이(Y축) 추가
        float arcY = Mathf.Sin(_progress * Mathf.PI) * _arcHeight;
        currentPos.y += arcY;

        // 진행 방향을 바라보도록 회전 업데이트
        Vector3 currentDirection = (currentPos - transform.position).normalized;
        if (currentDirection != Vector3.zero)
        {
            UpdateRotation(currentDirection);
        }

        transform.position = currentPos;

        if (_progress >= 1f)
        {
            OnDestinationReached();
        }
    }

    protected void UpdateRotation(Vector3 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);
    }

    /// <summary>
    /// 곡사포가 바닥(목표 지점)에 착탄했을 때 호출됩니다.
    /// </summary>
    protected virtual void OnDestinationReached()
    {
        // 기본적으로는 도착 시 파괴 (자식에서 폭발 효과 등으로 오버라이드 가능)
        Destroy(gameObject);
    }

    /// <summary>
    /// 충돌 감지 처리 (자식 클래스에서 데미지 로직 구현)
    /// </summary>
    protected abstract void OnTriggerEnter2D(Collider2D other);
}
