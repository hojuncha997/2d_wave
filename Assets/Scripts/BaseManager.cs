using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 게임의 핵심인 기지(Base)의 체력과 배치된 타워 슬롯들을 총괄 관리합니다.
/// </summary>
public class BaseManager : MonoBehaviour
{
    public static BaseManager Instance { get; private set; }

    [Header("Base Settings")]
    [SerializeField] private int _maxHp = 10;
    [SerializeField] private List<TowerSlot> _towerSlots = new List<TowerSlot>();

    private int _currentHp;
    private bool _isDestroyed = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _currentHp = _maxHp;
        
        // 만약 에디터에서 슬롯을 연결하지 않았다면 자식 오브젝트에서 자동으로 찾아옵니다.
        if (_towerSlots.Count == 0)
        {
            _towerSlots.AddRange(GetComponentsInChildren<TowerSlot>());
        }

        Debug.Log($"기지 시스템 가동. 현재 체력: {_currentHp}/{_maxHp}, 슬롯 수: {_towerSlots.Count}");
    }

    /// <summary>
    /// 기지가 적에게 공격받았을 때 호출됩니다.
    /// </summary>
    public void TakeDamage(int damage)
    {
        if (_isDestroyed) return;

        _currentHp -= damage;
        Debug.Log($"기지 피격! 남은 체력: {_currentHp}/{_maxHp}");

        if (_currentHp <= 0)
        {
            DestroyBase();
        }
    }

    private void DestroyBase()
    {
        _isDestroyed = true;
        Debug.Log("기지 파괴됨! 게임 오버.");

        // GameManager에게 게임 종료를 알립니다.
        if (GameManager.Instance != null)
        {
            GameManager.Instance.EndGame();
        }
    }

    /// <summary>
    /// 모든 타워 슬롯 리스트를 반환합니다.
    /// </summary>
    public List<TowerSlot> GetTowerSlots()
    {
        return _towerSlots;
    }

    // 적이 기지(하단 라인)에 도달했을 때의 처리를 위한 충돌 감지
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // 적이 기지에 도달하면 기지에 데미지를 입히고 적은 소멸됩니다.
            TakeDamage(1);

            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.Die();
            }
        }
    }
}
