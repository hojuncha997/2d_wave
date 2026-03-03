using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private float lifeTime = 2f; // 화면 밖으로 나가면 삭제하기 위함

    void Start()
    {
        // 2초 뒤에 자동으로 파괴 (메모리 관리)
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // 위로 이동
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }
}