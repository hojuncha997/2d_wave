using UnityEngine;

/// <summary>
/// 충돌 시 특정 범위 내의 모든 적에게 데미지를 입히는 폭발형 탄환입니다.
/// </summary>
public class ExplosiveBullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    [SerializeField] private int _damage = 2;       // 폭발 데미지
    [SerializeField] private float _speed = 10f;    // 탄환 속도
    [SerializeField] private float _lifeTime = 3f;  // 수명
    [SerializeField] private float _explosionRadius = 2f; // 폭발 범위

    [Header("Visual Effects")]
    [SerializeField] private GameObject _explosionEffectPrefab; // 폭발 시 파티클 (선택)

    private Transform _target;
    private Vector3 _lastDirection = Vector3.up;

    private void Start()
    {
        Destroy(gameObject, _lifeTime);
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    private void Update()
    {
        if (_target != null)
        {
            _lastDirection = (_target.position - transform.position).normalized;
            float angle = Mathf.Atan2(_lastDirection.y, _lastDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle - 90);
        }

        transform.position += _lastDirection * _speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Explode();
        }
    }

    /// <summary>
    /// 폭발 범위를 계산하여 데미지를 입히고 소멸합니다.
    /// </summary>
    private void Explode()
    {
        // 폭발 시각 효과 (프리팹이 있을 경우에만)
        if (_explosionEffectPrefab != null)
        {
            Instantiate(_explosionEffectPrefab, transform.position, Quaternion.identity);
        }

        // 폭발 범위 내의 모든 콜라이더를 찾음
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _explosionRadius);

        foreach (Collider2D col in colliders)
        {
            if (col.CompareTag("Enemy"))
            {
                Enemy enemy = col.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(_damage);
                }
            }
        }

        Debug.Log($"폭발 발생! 범위: {_explosionRadius}, 입힌 데미지: {_damage}");
        
        // 탄환 소멸
        Destroy(gameObject);
    }

    // 에디터에서 폭발 범위를 시각적으로 확인하기 위한 기능
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _explosionRadius);
    }
}
