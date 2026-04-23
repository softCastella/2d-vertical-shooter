using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    // ── 적 스폰 관련 ──────────────────────────────────────
    public GameObject[] enemies;        // 스폰할 적 프리팹 목록 (Inspector에서 할당)
    public GameObject[] enemySpawners;  // [0]: EnemySpawner0 (위쪽), [1~4]: EnemySpawner1~4 (좌우)
    private float delta = 0;            // 마지막 적 생성 이후 경과 시간
    private int index = 0;              // 생성된 적 오브젝트 이름용 고유 인덱스
    private bool isGameOver = false;    // 게임 오버 여부 (true면 스폰 중단)

    // ── UI 관련 ───────────────────────────────────────────
    public GameObject life_0;               // 라이프 아이콘 0 (마지막 라이프)
    public GameObject life_1;               // 라이프 아이콘 1
    public GameObject life_2;               // 라이프 아이콘 2 (첫 번째로 사라짐)
    public GameObject GameOverPanel;        // 게임 오버 패널
    public GameObject RetryButton;          // 리트라이 버튼
    public TextMeshProUGUI scoreText;       // 점수 텍스트 UI
    private int score;                      // 현재 점수
    private GameObject enemy;              // 점수 계산용 적 오브젝트 참조

    void Start()
    {
        // 씬에서 UI 오브젝트 찾아 초기 상태 설정
        GameOverPanel = GameObject.Find("GameOverPanel");
        RetryButton = GameObject.Find("RetryButton");
        GameOverPanel.SetActive(false);
        RetryButton.SetActive(false);
        life_0.SetActive(true);
        life_1.SetActive(true);
        life_2.SetActive(true);

        enemy = GameObject.Find("Enemy");
    }

    void Update()
    {
        // 게임 오버 상태면 스폰 중단
        if (isGameOver) return;

        delta += Time.deltaTime;

        // 1~3초 간격으로 랜덤하게 적 생성
        if (delta > Random.Range(1f, 3f))
        {
            CreateEnemy();
            delta = 0;
        }
    }

    // 플레이어 HP 0 → 라이프 1 감소, 모두 소진 시 GameOver
    public void PlayerDied()
    {
        if (life_2 != null && life_2.activeSelf)       life_2.SetActive(false);
        else if (life_1 != null && life_1.activeSelf)  life_1.SetActive(false);
        else if (life_0 != null && life_0.activeSelf)  life_0.SetActive(false);

        if (!life_0.activeSelf && !life_1.activeSelf && !life_2.activeSelf)
            GameOver();
    }


    // 씬의 모든 적·총알 제거 후 게임 오버 UI 표시
    public void GameOver()
    {
        isGameOver = true;

        // 씬에 남아있는 모든 적 제거
        foreach (Enemy e in FindObjectsByType<Enemy>(FindObjectsSortMode.None))
            Destroy(e.gameObject);

        // 씬에 날아다니는 모든 적 총알 제거
        foreach (EnemyBullet b in FindObjectsByType<EnemyBullet>(FindObjectsSortMode.None))
            Destroy(b.gameObject);

        GameOverPanel.SetActive(true);
        RetryButton.SetActive(true);
    }

    // 리트라이 버튼 클릭 시 씬 리로드 → 라이프·HP 모두 초기화
    public void onRetryButtonClick()
    {
        SceneManager.LoadScene("GameScene");
    }

    // 적 처치 시 점수 누적 및 UI 갱신
    public void AddScore(int exp)
    {
        score += exp;
        Debug.Log($"Score: {score}");
        if (scoreText != null)
            scoreText.text = score.ToString("#,##0");
    }

    // 랜덤 스포너에서 랜덤 적 프리팹을 생성
    void CreateEnemy()
    {
        if (enemies.Length == 0 || enemySpawners.Length == 0) return;

        GameObject prefab = enemies[Random.Range(0, enemies.Length)];
        GameObject enemyGo = null;

        // EnemySpawner0(위쪽)과 EnemySpawner1~4(좌우) 중 랜덤 선택
        int spawnerIndex = Random.Range(0, enemySpawners.Length);
        GameObject spawner = enemySpawners[spawnerIndex];

        if (spawnerIndex == 0)
        {
            // EnemySpawner0: spawnPoint_0~4 중 랜덤 위치에서 아래 방향으로 생성
            List<Transform> spawnPoints = new List<Transform>();
            foreach (Transform child in spawner.transform)
            {
                if (child.name.StartsWith("spawnPoint"))
                    spawnPoints.Add(child);
            }
            if (spawnPoints.Count == 0) return;

            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
            enemyGo = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
            enemyGo.GetComponent<Enemy>().moveDirection = Vector3.down;
        }
        else
        {
            // EnemySpawner1~4: startPoint → endPoint 방향으로 이동
            Transform startPoint = spawner.transform.Find("startPoint");
            Transform endPoint = spawner.transform.Find("endPoint");

            if (startPoint == null || endPoint == null) return;

            Vector3 dir = (endPoint.position - startPoint.position).normalized;
            float zRot = dir.x > 0 ? 90f : -90f; // 오른쪽이면 +90, 왼쪽이면 -90 회전
            enemyGo = Instantiate(prefab, startPoint.position, Quaternion.Euler(0, 0, zRot));
            enemyGo.GetComponent<Enemy>().moveDirection = dir;
        }

        // 생성된 적 이름에 고유 인덱스 부여
        enemyGo.name = $"{enemyGo.name}_{index}";
        index++;
    }
}
