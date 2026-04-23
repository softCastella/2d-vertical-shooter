using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCont : MonoBehaviour
{
    // ── 발사 관련 ─────────────────────────────────────────
    public Transform firePoint;             // 총알이 발사되는 위치 (총구)
    public GameObject playerBulletPrefab0;  // 가운데 총알 프리팹 (power 3에서 사용)
    public GameObject playerBulletPrefab1;  // 좌우 총알 프리팹
    public float sideOffset;               // 좌우 총알 발사 간격
    public float power = 1;                // 현재 파워 단계 (1~3)

    // ── 이동 관련 ─────────────────────────────────────────
    public float speed = 3;                // 플레이어 이동 속도
    private float minX, maxX, minY, maxY;  // 화면 경계 (이동 범위 제한용)

    // ── 스탯 ──────────────────────────────────────────────
    public int hp = 100;
    public int maxHp = 100;

    // ── 참조 ──────────────────────────────────────────────
    private Animator anim;
    private GameManager gameManager;

    // 플레이어 이동 상태 (애니메이터 파라미터 연동)
    public enum PlayerState
    {
        Idle,   // 0: 정지
        Left,   // 1: 왼쪽 이동
        Right   // 2: 오른쪽 이동
    }

    void Start()
    {
        Debug.Log($"firePoint:{firePoint.position}");
        setBounds();
        anim = GetComponent<Animator>();
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        Move();
        ClampPosition();

        // A 키로 파워 단계 전환 (1 → 2 → 3 → 1 반복)
        if (Input.GetKeyDown(KeyCode.A))
        {
            power++;
            if (power > 3) power = 1;
            Debug.Log($"파워 단계: {power}");
        }

        // Space 키로 발사
        if (Input.GetKeyDown(KeyCode.Space))
            Shoot();
    }

    // 카메라 뷰포트 기준으로 이동 가능한 화면 경계 계산
    void setBounds()
    {
        Camera cam = Camera.main;
        Vector3 bottomL = cam.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 topR = cam.ViewportToWorldPoint(new Vector3(1, 1, 0));

        // 스프라이트 크기의 절반만큼 여백을 두어 화면 밖으로 잘리지 않도록 함
        Vector2 halfSize = Vector2.zero;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
            halfSize = sr.bounds.extents;

        minX = bottomL.x + halfSize.x;
        maxX = topR.x - halfSize.x;
        minY = bottomL.y + halfSize.y;
        maxY = topR.y - halfSize.y;
    }

    void Move()
    {
        // GetAxisRaw: 스무딩 없이 -1, 0, 1 즉시 반환 → 입력 반응이 즉각적
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 dir = new Vector3(h, v, 0);

        // 대각선 입력 시 속도가 √2배 빨라지는 것을 방지
        if (dir.magnitude > 1f) dir.Normalize();

        transform.Translate(dir * speed * Time.deltaTime);

        // 수평 입력 방향에 따라 애니메이션 상태 변경
        PlayerState state;
        if (h < 0)       state = PlayerState.Left;
        else if (h > 0)  state = PlayerState.Right;
        else             state = PlayerState.Idle;

        anim.SetInteger("state", (int)state);
    }

    // 플레이어 위치를 화면 경계 안으로 제한
    void ClampPosition()
    {
        float x = Mathf.Clamp(transform.position.x, minX, maxX);
        float y = Mathf.Clamp(transform.position.y, minY, maxY);
        transform.position = new Vector3(x, y, transform.position.z);
    }

    // 파워 단계에 따라 총알 발사 패턴이 달라짐
    void Shoot()
    {
        if (power == 1)
        {
            // 파워 1: 총구 중앙에서 1발
            GameObject bullet = Instantiate(playerBulletPrefab1);
            bullet.transform.position = firePoint.position;
        }
        else if (power == 2)
        {
            // 파워 2: 좌우 2발
            sideOffset = 0.13f;
            GameObject leftBullet = Instantiate(playerBulletPrefab1);
            GameObject rightBullet = Instantiate(playerBulletPrefab1);
            leftBullet.transform.position  = firePoint.position + Vector3.left  * sideOffset;
            rightBullet.transform.position = firePoint.position + Vector3.right * sideOffset;
        }
        else if (power == 3)
        {
            // 파워 3: 가운데 1발 + 좌우 2발, 총 3발
            sideOffset = 0.25f;
            GameObject centerBullet = Instantiate(playerBulletPrefab0);
            GameObject leftBullet   = Instantiate(playerBulletPrefab1);
            GameObject rightBullet  = Instantiate(playerBulletPrefab1);
            centerBullet.transform.position = firePoint.position;
            leftBullet.transform.position   = firePoint.position + Vector3.left  * sideOffset;
            rightBullet.transform.position  = firePoint.position + Vector3.right * sideOffset;
        }
    }

    // 적 총알에서 호출 - HP 감소 후 사망 판정
    public void TakeDamage(int damage)
    {
        hp -= damage;
        Debug.Log($"HP: {hp} / {maxHp}");
        Die();
    }

    // hp가 0 이하면 플레이어 오브젝트 제거 후 게임 오버 처리
    void Die()
    {
        if (hp <= 0)
        {
            hp = 0;
            Debug.Log("Player is dead");
            Destroy(gameObject);
            gameManager.GameOver();
        }
    }

    // 적과 직접 충돌 시 플레이어·적 둘 다 제거 후 게임 오버
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject); // 적 제거
            Destroy(gameObject);       // 플레이어 제거
            gameManager.GameOver();
        }
    }
}
