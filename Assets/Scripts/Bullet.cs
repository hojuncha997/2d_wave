using UnityEngine;

/// <summary>
/// 특정 대상을 자동으로 추적하여 명중시키는 총알 클래스입니다.
/// </summary>
public class Bullet : MonoBehaviour
{
    [SerializeField] private float _speed = 15f;
    [SerializeField] private float _lifeTime = 3f;

    private Transform _target;

    private void Start()
    {
        // 안전을 위해 일정 시간 뒤 파괴
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
        // 타겟이 이미 파괴되었거나 사라졌다면 총알도 함께 파괴
        if (_target == null)
        {
            Destroy(gameObject);
            return;
        }

        // 타겟 방향 계산
        Vector3 direction = (_target.position - transform.position).normalized;
        
        // 타겟을 향해 이동
        transform.position += direction * _speed * Time.deltaTime;

        // 총알이 타겟 방향을 바라보도록 회전 (2D)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90); // 90도 조정은 스프라이트의 기본 방향에 따름
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 충돌한 물체의 태그가 "Enemy"인 경우
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.Die();
            }

            // 명중 후 자신 파괴
            Destroy(gameObject);
        }
    }
}
