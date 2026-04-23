// using UnityEngine;
// using UnityEngine.SceneManagement;
// using TMPro;

// public class Test1 : MonoBehaviour
// {
//     public GameObject life_0;
//     public GameObject life_1;
//     public GameObject life_2;
//     public GameObject GameOverPanel;
//     public GameObject RetryButton;
//     public TextMeshProUGUI scoreText;
//     private int score;

//     void Start()
//     {
//         GameOverPanel = GameObject.Find("GameOverPanel");
//         RetryButton = GameObject.Find("RetryButton");
//         GameOverPanel.SetActive(false);
//         RetryButton.SetActive(false);
//         life_0.SetActive(true);
//         life_1.SetActive(true);
//         life_2.SetActive(true);
//     }

//     // 버튼 클릭 시 life_2 → life_1 → life_0 순으로 하나씩 숨김
//     // 모두 꺼지면 GameOverPanel 표시
//     public void onDieButtonClick()
//     {
//         Debug.Log($"onDieButtonClick 호출됨 / life_2:{life_2} life_1:{life_1} life_0:{life_0}");

//         if (life_2 != null && life_2.activeSelf)
//         {
//             Debug.Log("life_2 숨김");
//             life_2.SetActive(false);
//         }
//         else if (life_1 != null && life_1.activeSelf)
//         {
//             Debug.Log("life_1 숨김");
//             life_1.SetActive(false);
//         }
//         else if (life_0 != null && life_0.activeSelf)
//         {
//             Debug.Log("life_0 숨김");
//             life_0.SetActive(false);
//         }

//         // life 셋 다 꺼졌으면 게임 오버
//         if (!life_0.activeSelf && !life_1.activeSelf && !life_2.activeSelf)
//         {
//             GameOverPanel.SetActive(true);
//             RetryButton.SetActive(true);
//         }
//     }

//     public void onRetryButtonClick()
//     {
//         // 씬 리로드 시 모든 오브젝트가 초기화되므로 추가 초기화 불필요
//         SceneManager.LoadScene("GameScene");
//     }

//     //대리자 메서드 : 기다리고 있다가 나중에 콜백으로 호출해준다.
//      public void OnAddScoreButtonClick()
//     {
//         score += 100;
//         scoreText.text = score.ToString("#,##0");
//     }

// }
