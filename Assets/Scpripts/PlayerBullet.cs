using UnityEngine;

// 화면 이탈 제거는 OutOfBoundsDestroy 컴포넌트가 담당
public class PlayerBullet : MonoBehaviour
{
    [SerializeField] private float speed = 10f; // 총알 이동 속도
    public int damage = 10;

    void Update()
    {
        // 매 프레임 위쪽으로 이동
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }
}
