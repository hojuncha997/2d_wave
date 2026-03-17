using UnityEngine;

/// <summary>
/// 특정 대상을 추적하여 단일 타겟에게 명중시키는 기본 총알 클래스입니다.
/// </summary>
public class Bullet : BaseProjectile
{
    [Header("Bullet Settings")]
    [SerializeField] private int _damage = 1;
    [Tooltip("타겟이 사라졌을 때 총알을 즉시 파괴할지 여부 (유도탄 등에 활용 가능)")]
    [SerializeField] private bool _destroyOnTargetLost = false;

    protected override void Update()
    {
        // 타겟이 유실되었을 때의 특수 처리 로직 (부모 로직 확장)
        if (_target == null && _destroyOnTargetLost)
        {
            Destroy(gameObject);
            return;
        }

        base.Update();
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        // 충돌한 물체의 태그가 "Enemy"인 경우 데미지를 입히고 소멸
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(_damage);
            }

            Destroy(gameObject);
        }
    }
}
