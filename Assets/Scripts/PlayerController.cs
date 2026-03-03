using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 15f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float fireRate = 0.3f;

    private float screenHalfWidth;
    private float fireTimer;
    private Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;
        // 카메라 기준 화면 가장자리 x좌표 계산
        screenHalfWidth = mainCam.orthographicSize * mainCam.aspect;
    }

    void Update()
    {
        HandleMovement();
        HandleFiring();
    }

    void HandleMovement()
    {
        float inputX = 0f;

        // 터치 입력 (모바일)
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPos = mainCam.ScreenToWorldPoint(touch.position);
            float diff = touchPos.x - transform.position.x;
            inputX = Mathf.Clamp(diff, -1f, 1f);
        }
        // 키보드 입력 (에디터 테스트용)
        else
        {
            inputX = Input.GetAxisRaw("Horizontal");
        }

        Vector3 pos = transform.position;
        pos.x += inputX * moveSpeed * Time.deltaTime;
        pos.x = Mathf.Clamp(pos.x, -screenHalfWidth + 0.5f, screenHalfWidth - 0.5f);
        transform.position = pos;
    }

    void HandleFiring()
    {
        fireTimer += Time.deltaTime;
        if (fireTimer >= fireRate)
        {
            fireTimer = 0f;
            if (bulletPrefab != null)
            {
                Vector3 spawnPos = transform.position + Vector3.up * 0.6f;
                Instantiate(bulletPrefab, spawnPos, Quaternion.identity);
            }
        }
    }
}