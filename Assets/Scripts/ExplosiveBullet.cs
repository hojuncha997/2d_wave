using UnityEngine;

/// <summary>
/// 충돌 시 특정 범위 내의 모든 적에게 데미지를 입히는 폭발형 탄환입니다.
/// 부모 클래스의 포물선 이동 기능을 활용할 수 있습니다.
/// </summary>
public class ExplosiveBullet : BaseProjectile
{
    [Header("Explosive Settings")]
    [SerializeField] private int _damage = 2;       // 폭발 데미지
    [SerializeField] private float _explosionRadius = 2f; // 폭발 범위

    [Header("Visual Effects")]
    [SerializeField] private GameObject _explosionEffectPrefab; // 폭발 시 파티클

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        // 일반적인 충돌 시 폭발
        if (other.CompareTag("Enemy"))
        {
            Explode();
        }
    }

    protected override void SpawnLandingMarker()
    {
        base.SpawnLandingMarker();

        // 마커가 생성되었다면 폭발 반경에 맞춰 크기 조절
        if (_spawnedMarker != null)
        {
            float diameter = _explosionRadius * 2f;
            _spawnedMarker.transform.localScale = new Vector3(diameter, diameter, 1f);
        }
    }

    protected override void OnDestinationReached()
    {
        // 곡사포 모드일 때 목표 지점에 도달하면 폭발
        Explode();
    }

    /// <summary>
    /// 폭발 범위를 계산하여 데미지를 입히고 소멸합니다.
    /// </summary>
    private void Explode()
    {
        if (_explosionEffectPrefab != null)
        {
            Instantiate(_explosionEffectPrefab, transform.position, Quaternion.identity);
        }

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
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _explosionRadius);
    }
}
