using UnityEngine;

/// <summary>
/// 화면 위에서 아래로 이동하는 적의 기본 로직을 담당합니다.
/// </summary>
public class Enemy : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private int _goldReward = 10; // 추가: 적 처치 시 골드 보상
    [SerializeField] private float _lifeTime = 10f;

    private void Start()
    {
        Destroy(gameObject, _lifeTime);
    }

    private void Update()
    {
        transform.Translate(Vector3.down * _moveSpeed * Time.deltaTime);
    }

    /// <summary>
    /// 외부에서 적이 파괴될 때 호출되는 메서드 (예: 총알에 맞았을 때)
    /// </summary>
    public void Die()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddScore(100);
            GameManager.Instance.AddGold(_goldReward); // 추가: 골드 보상 지급
        }

        Destroy(gameObject);
    }
}
