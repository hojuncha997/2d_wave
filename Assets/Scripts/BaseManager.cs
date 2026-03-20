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

    // UI에서 접근할 수 있도록 속성 추가
    public int CurrentHp => _currentHp;
    public int MaxHp => _maxHp;

    // 체력 변경 시 UI를 업데이트하기 위한 델리게이트/이벤트
    public System.Action<int, int> OnHealthChanged;

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
        OnHealthChanged?.Invoke(_currentHp, _maxHp);
        
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
        Debug.Log($"[BaseManager] TakeDamage 진입! (파괴상태: {_isDestroyed}, 데미지: {damage})");
        
        if (_isDestroyed) 
        {
            Debug.Log("[BaseManager] 기지가 이미 파괴된 상태라 데미지를 무시합니다.");
            return;
        }

        _currentHp -= damage;
        OnHealthChanged?.Invoke(_currentHp, _maxHp);
        Debug.Log($"[BaseManager] 체력 감소 완료! 남은 체력: {_currentHp}/{_maxHp}");

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
        try 
        {
            if (other == null) return;
            
            Debug.Log($"[BaseManager] 충돌 감지: {other.name}, 태그: {other.tag}");

            if (other.CompareTag("Enemy"))
            {
                Debug.Log("[BaseManager] 적(Enemy) 확인됨! 데미지 처리 시작.");
                TakeDamage(1);

                // 적 제거
                Enemy enemy = other.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.Die();
                }
                else
                {
                    Debug.LogWarning($"[BaseManager] {other.name}에 Enemy 컴포넌트가 없습니다. 파괴만 진행합니다.");
                    Destroy(other.gameObject);
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[BaseManager] OnTriggerEnter2D 오류 발생: {e.Message}\n{e.StackTrace}");
        }
    }
}
