using UnityEngine;

/// <summary>
/// 플레이어가 발사하는 총알의 이동과 충돌을 담당합니다.
/// </summary>
public class Bullet : MonoBehaviour
{
    [SerializeField] private float _speed = 20f;
    [SerializeField] private float _lifeTime = 2f; // 화면 밖으로 나가면 삭제하기 위함

    private void Start()
    {
        // 일정 시간 뒤에 자동으로 파괴 (메모리 관리)
        Destroy(gameObject, _lifeTime);
    }

    private void Update()
    {
        // 위로 이동
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 충돌한 물체의 태그가 "Enemy"인 경우
        if (other.CompareTag("Enemy"))
        {
            // 적 객체의 Die() 메서드 호출
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.Die();
            }

            // 총알 자신 파괴
            Destroy(gameObject);
        }
    }
}
