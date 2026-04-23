using UnityEngine;

public class Enemy : MonoBehaviour
{
    // 적 타입 (Inspector에서 설정)
    public enum EnemyType { A, B, C }
    public EnemyType enemyType;

    private SpriteRenderer sr;
    private Transform playerTransform;
    private Collider2D playerCollider;
    public GameObject enemyBulletPrefab0;
    public GameObject enemyBulletPrefab1;
    public Sprite[] sprites;           // [0] 기본 스프라이트, [1] 피격 스프라이트
    private Transform[] firePoints;          // 총알이 발사되는 위치 - Start에서 자동 탐색
    public int health;                 // 적의 체력
    public float power;                // 적의 데미지
    public float speed;                // 이동 속도
    public int exp;                  //경험치 -> 스코어 합산
    
    [HideInInspector]
    public Vector3 moveDirection = Vector3.down; // 이동 방향 (GameManager에서 설정)

    private float shootInterval = 99f; // 발사 간격 (초) - Start()에서 타입별로 덮어씀
    private float shootTimer;          // 발사 타이머

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if (sprites.Length > 0)
            sr.sprite = sprites[0];

        GameObject playerGo = GameObject.FindWithTag("Player");
        if (playerGo != null)
        {
            playerTransform = playerGo.transform;
            playerCollider = playerGo.GetComponent<Collider2D>();
        }

        // 자식 오브젝트 중 이름에 FirePoint 포함된 것 전부 수집
        var fps = new System.Collections.Generic.List<Transform>();
        foreach (Transform child in transform)
            if (child.name.Contains("FirePoint"))
                fps.Add(child);
        firePoints = fps.ToArray();

        // 타입에 따라 스탯 설정 (Start에서 한 번만 실행)
        switch (enemyType)
        {
            case EnemyType.A:
                health = 80; power = 1f; speed = 1f; exp = 10; shootInterval = 2f; //적당히 빠른애애
                break; 
            case EnemyType.B:
                health = 100; power = 1.5f; speed = 8f; exp = 15; shootInterval = 1.5f; //가장 빠른애
                break;
            case EnemyType.C:
                health = 200; power = 3f; speed = 0.5f; exp = 20; shootInterval = 3f;  //가장 느린애
                break;
        }
    }

    void Update()
    {
        // 매 프레임 설정된 방향으로 이동 (Space.World: 오브젝트 회전에 영향받지 않음)
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);

        // 발사 타이머 누적 후 간격마다 총알 발사 (shootInterval이 0이면 발사 안 함)
        if (shootInterval > 0f)
        {
            shootTimer += Time.deltaTime;
            if (shootTimer >= shootInterval)
            {
                // if(enemyType == EnemyType.A || enemyType == EnemyType.B)
                //     ShootOne();
                // else 
                if(enemyType == EnemyType.C)
                    ShotTwo();
                
                shootTimer = 0f;
            }
        }
    }

    // 피격 처리: 체력 감소 → 피격 스프라이트 표시 → 사망 판정
    private void Hit(int damage)
    {
        health -= damage;

        // 0.1초간 피격 스프라이트로 변경 후 원래대로 복구
        sr.sprite = sprites[1];
        Invoke("ReturnDefaultSprite", 0.1f);

        // 체력이 0 이하면 점수 합산 후 오브젝트 제거
        if (health <= 0)
        {
            GameManager gm = FindObjectOfType<GameManager>();
            if (gm != null) gm.AddScore(exp);
            Destroy(gameObject);
        }
    }

    // 기본 스프라이트로 되돌리기 (Invoke로 0.1초 후 호출됨)
    private void ReturnDefaultSprite()
    {
        sr.sprite = sprites[0];
    }

    Vector3 AimAtPlayer(Vector3 fromPosition)
    {
        if (playerTransform == null) return moveDirection;
        Vector3 target = playerCollider != null ? playerCollider.bounds.center : playerTransform.position;
        return (target - fromPosition).normalized;
    }

    void ShotTwo()
    {
        GameObject b0 = Instantiate(enemyBulletPrefab1, firePoints[0].position, Quaternion.identity);
        b0.GetComponent<EnemyBullet>().moveDirection = AimAtPlayer(firePoints[0].position);

        GameObject b1 = Instantiate(enemyBulletPrefab1, firePoints[1].position, Quaternion.identity);
        b1.GetComponent<EnemyBullet>().moveDirection = AimAtPlayer(firePoints[1].position);
    }

    void ShootOne()
    {
        GameObject b = Instantiate(enemyBulletPrefab0, firePoints[0].position, Quaternion.identity);
        b.GetComponent<EnemyBullet>().moveDirection = AimAtPlayer(firePoints[0].position);
    }

    // 충돌 처리: 플레이어 총알 → 피격 / 플레이어 본체 → 둘 다 제거
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PlayerBullet"))
        {
            PlayerBullet playerBullet = other.gameObject.GetComponent<PlayerBullet>();
            Hit(playerBullet.damage);
            Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag("Player"))
        {
            Destroy(other.gameObject); // 플레이어 제거
            Destroy(gameObject);       // 적 제거

            // 씬의 GameManager를 찾아 게임 오버 처리
            GameManager gm = FindObjectOfType<GameManager>();
            if (gm != null) gm.GameOver();
        }
    }
}
