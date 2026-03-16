using UnityEngine;

/// <summary>
/// 사거리 내의 적을 자동으로 감지하고 사격하는 타워의 기본 클래스입니다.
/// </summary>
public class Tower : MonoBehaviour
{
    [Header("Tower Settings")]
    [SerializeField] private float _fireRate = 1.0f; // 초당 발사 횟수
    [SerializeField] private float _range = 5.0f;    // 사거리
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _firePoint;   // 총알이 발사되는 위치

    private float _fireTimer;
    private Transform _target;

    private void Update()
    {
        UpdateTarget();
        HandleFiring();
    }

    /// <summary>
    /// 사거리 내에서 가장 가까운 적을 찾아 타겟으로 설정합니다.
    /// </summary>
    private void UpdateTarget()
    {
        // 모든 적을 찾음
        Enemy[] enemies = Object.FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        float shortestDistance = Mathf.Infinity;
        Transform nearestEnemy = null;

        foreach (Enemy enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy.transform;
            }
        }

        // 가장 가까운 적이 사거리 내에 있는지 확인
        if (nearestEnemy != null && shortestDistance <= _range)
        {
            _target = nearestEnemy;
        }
        else
        {
            _target = null;
        }
    }

    private void HandleFiring()
    {
        if (_target == null) 
        {
            _fireTimer = 1f / _fireRate;
            return;
        }

        _fireTimer += Time.deltaTime;
        if (_fireTimer >= 1f / _fireRate)
        {
            _fireTimer = 0f;
            Shoot();
        }
    }

    private void Shoot()
    {
        if (_bulletPrefab == null || _firePoint == null || _target == null) return;

        // 총알 생성
        GameObject bulletObj = Instantiate(_bulletPrefab, _firePoint.position, Quaternion.identity);

        // 1. 일반 총알(Bullet)인 경우 타겟 설정
        Bullet bullet = bulletObj.GetComponent<Bullet>();
        if (bullet != null)
        {
            bullet.SetTarget(_target);
            return;
        }

        // 2. 폭발형 총알(ExplosiveBullet)인 경우 타겟 설정
        ExplosiveBullet expBullet = bulletObj.GetComponent<ExplosiveBullet>();
        if (expBullet != null)
        {
            expBullet.SetTarget(_target);
            return;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _range);
    }
}
