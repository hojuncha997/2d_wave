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
        // 모든 적을 찾음 (성능을 위해 추후 레이어 마스크나 오버랩 서클로 최적화 가능)
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
        if (_target == null) return;

        _fireTimer += Time.deltaTime;
        if (_fireTimer >= 1f / _fireRate)
        {
            _fireTimer = 0f;
            Shoot();
        }
    }

    private void Shoot()
    {
        if (_bulletPrefab == null || _firePoint == null) return;

        // 총알 생성 및 발사 (타겟 방향으로 회전시키거나 단순히 위로 쏘게 할 수 있음)
        // 여기서는 기존 Bullet 로직(위로 이동)을 활용하기 위해 위쪽으로 생성
        GameObject bulletObj = Instantiate(_bulletPrefab, _firePoint.position, Quaternion.identity);
        
        // 만약 총알이 타겟을 향해 날아가게 하고 싶다면 여기서 방향을 설정할 수 있습니다.
        // 현재 Bullet.cs는 단순히 위로 올라가므로, 일단은 기본 설정을 유지합니다.
    }

    // 에디터에서 사거리를 시각적으로 확인하기 위한 기능
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _range);
    }
}
