using UnityEngine;

/// <summary>
/// 화면 위에서 아래로 이동하는 적의 기본 로직을 담당합니다.
/// </summary>
public class Enemy : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int _maxHp = 1;
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private int _goldReward = 10;
    [SerializeField] private float _lifeTime = 10f;

    private int _currentHp;

    private void Start()
    {
        _currentHp = _maxHp;
        Destroy(gameObject, _lifeTime);
    }

    private void Update()
    {
        transform.Translate(Vector3.down * _moveSpeed * Time.deltaTime);
    }

    /// <summary>
    /// 적에게 데미지를 입힙니다. 체력이 0 이하가 되면 사망합니다.
    /// </summary>
    public void TakeDamage(int damage)
    {
        _currentHp -= damage;
        Debug.Log($"{gameObject.name} 피격! 남은 체력: {_currentHp}/{_maxHp}");

        if (_currentHp <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// 적이 사망할 때 호출되는 메서드입니다.
    /// </summary>
    public void Die()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddScore(100);
            GameManager.Instance.AddGold(_goldReward);
        }

        Destroy(gameObject);
    }
}
