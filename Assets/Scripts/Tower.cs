using UnityEngine;

/// <summary>
/// 사거리 내의 적을 자동으로 감지하고 사격하는 타워의 기본 클래스입니다.
/// </summary>
public class Tower : MonoBehaviour
{
    [Header("Tower Stats")]
    [SerializeField] private int _damage = 1;
    [SerializeField] private float _fireRate = 1.0f; // 초당 발사 횟수
    [SerializeField] private float _range = 5.0f;    // 사거리
    
    [Header("Upgrade Settings")]
    [SerializeField] private int _level = 1;
    [SerializeField] private int _upgradeCost = 75;
    [SerializeField] private float _upgradeMultiplier = 1.2f; // 업그레이드 시 능력치 상승 배율

    [Header("References")]
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _firePoint;   // 총알이 발사되는 위치

    public int UpgradeCost => _upgradeCost;
    public int Level => _level;

    private float _fireTimer;
    private Transform _target;

    /// <summary>
    /// 타워의 레벨을 올리고 능력치를 강화합니다.
    /// </summary>
    public void Upgrade()
    {
        _level++;
        _damage = Mathf.RoundToInt(_damage * _upgradeMultiplier);
        _range *= 1.1f;
        _fireRate *= 1.1f;
        _upgradeCost = Mathf.RoundToInt(_upgradeCost * 1.5f);
        
        Debug.Log($"타워 업그레이드 완료! Level: {_level}, Damage: {_damage}, Range: {_range:F1}");
    }

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
        if (_target == null) 
        {
            // 타겟이 없을 때는 타이머를 '발사 준비 완료' 상태로 유지
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

        // 생성된 총알의 Bullet 컴포넌트를 가져와서 타겟 및 데미지 설정
        Bullet bullet = bulletObj.GetComponent<Bullet>();
        if (bullet != null)
        {
            bullet.SetTarget(_target);
            bullet.SetDamage(_damage); // 타워의 현재 데미지 적용
        }
    }

    // 에디터에서 사거리를 시각적으로 확인하기 위한 기능
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _range);
    }
}
