using UnityEngine;

/// <summary>
/// 화면 위에서 아래로 이동하는 적의 기본 로직을 담당합니다.
/// </summary>
public class Enemy : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _lifeTime = 10f; // 화면 밖으로 완전히 나갔을 때를 대비한 안전장치

    private void Start()
    {
        // 일정 시간 후 파괴 (성능 관리)
        Destroy(gameObject, _lifeTime);
    }

    private void Update()
    {
        // 아래로 이동
        transform.Translate(Vector3.down * _moveSpeed * Time.deltaTime);
    }

    /// <summary>
    /// 외부에서 적이 파괴될 때 호출되는 메서드 (예: 총알에 맞았을 때)
    /// </summary>
    public void Die()
    {
        // 점수 추가 (한 마리당 100점)
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddScore(100);
        }

        // 추후 여기에 폭발 이펙트나 사운드 추가 가능
        Destroy(gameObject);
    }
}
