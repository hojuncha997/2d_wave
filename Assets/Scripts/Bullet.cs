using UnityEngine;

/// <summary>
/// 특정 대상을 자동으로 추적하여 명중시키는 총알 클래스입니다.
/// </summary>
public class Bullet : MonoBehaviour
{
    [SerializeField] private int _damage = 1;
    [SerializeField] private float _speed = 15f;
    [SerializeField] private float _lifeTime = 3f;
    [Tooltip("타겟이 사라졌을 때 총알을 즉시 파괴할지 여부 (유도탄 등에 활용 가능)")]
    [SerializeField] private bool _destroyOnTargetLost = false;

    private Transform _target;
    private Vector3 _lastDirection = Vector3.up;

    private void Start()
    {
        // 안전을 위해 일정 시간 뒤 파괴 (이미 발사된 총알은 타겟 유무와 상관없이 수명만큼 날아감)
        Destroy(gameObject, _lifeTime);
    }

    /// <summary>
    /// 추적할 대상을 설정합니다.
    /// </summary>
    public void SetTarget(Transform target)
    {
        _target = target;
    }

    private void Update()
    {
        // 타겟이 살아있다면 방향 갱신 및 회전 업데이트
        if (_target != null)
        {
            _lastDirection = (_target.position - transform.position).normalized;

            // 총알이 타겟 방향을 바라보도록 회전 (2D)
            float angle = Mathf.Atan2(_lastDirection.y, _lastDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle - 90);
        }
        else if (_destroyOnTargetLost)
        {
            // 타겟이 사라졌고, 옵션이 켜져 있다면 즉시 파괴
            Destroy(gameObject);
            return;
        }

        // 타겟 존재 여부와 상관없이 마지막 방향(또는 갱신된 방향)으로 전진
        transform.position += _lastDirection * _speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 충돌한 물체의 태그가 "Enemy"인 경우
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(_damage);
            }

            // 명중 후 자신 파괴
            Destroy(gameObject);
        }
    }
}
