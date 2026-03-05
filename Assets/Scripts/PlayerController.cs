using UnityEngine;

/// <summary>
/// 플레이어의 이동, 발사 및 체력을 관리합니다.
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("Movement & Firing")]
    [SerializeField] private float _moveSpeed = 15f;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private float _fireRate = 0.3f;

    [Header("Health")]
    [SerializeField] private int _maxHp = 3;
    private int _currentHp;

    private float _screenHalfWidth;
    private float _fireTimer;
    private Camera _mainCam;
    private bool _isDead = false;

    private void Start()
    {
        _mainCam = Camera.main;
        _screenHalfWidth = _mainCam.orthographicSize * _mainCam.aspect;
        _currentHp = _maxHp;
    }

    private void Update()
    {
        if (_isDead) return;

        HandleMovement();
        HandleFiring();
    }

    private void HandleMovement()
    {
        float inputX = 0f;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPos = _mainCam.ScreenToWorldPoint(touch.position);
            float diff = touchPos.x - transform.position.x;
            inputX = Mathf.Clamp(diff, -1f, 1f);
        }
        else
        {
            inputX = Input.GetAxisRaw("Horizontal");
        }

        Vector3 pos = transform.position;
        pos.x += inputX * _moveSpeed * Time.deltaTime;
        pos.x = Mathf.Clamp(pos.x, -_screenHalfWidth + 0.5f, _screenHalfWidth - 0.5f);
        transform.position = pos;
    }

    private void HandleFiring()
    {
        _fireTimer += Time.deltaTime;
        if (_fireTimer >= _fireRate)
        {
            _fireTimer = 0f;
            if (_bulletPrefab != null)
            {
                Vector3 spawnPos = transform.position + Vector3.up * 0.6f;
                Instantiate(_bulletPrefab, spawnPos, Quaternion.identity);
            }
        }
    }

    /// <summary>
    /// 플레이어가 데미지를 입을 때 호출됩니다.
    /// </summary>
    public void TakeDamage(int damage)
    {
        if (_isDead) return;

        _currentHp -= damage;
        Debug.Log($"플레이어 체력: {_currentHp}/{_maxHp}");

        if (_currentHp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        _isDead = true;
        Debug.Log("플레이어 사망!");
        
        // GameManager에게 게임 오버 알림
        if (GameManager.Instance != null)
        {
            GameManager.Instance.EndGame();
        }

        // 플레이어 오브젝트를 비활성화하거나 파괴 (여기선 일단 비활성화)
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"무언가와 충돌함: {other.name}, 태그: {other.tag}");

        // 적과 직접 충돌했을 때
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("적과 충돌함! 데미지를 입습니다.");
            TakeDamage(1);
            
            // 충돌한 적도 파괴 처리
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.Die();
            }
        }
    }
}
