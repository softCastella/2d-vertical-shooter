using UnityEngine;

// 화면 밖으로 나가면 자동으로 오브젝트를 제거하는 범용 컴포넌트
// 적, 총알 등 화면 밖으로 사라져야 하는 프리팹에 붙여서 사용
public class OutOfBoundsDestroy : MonoBehaviour
{
    void Update()
    {
        Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);
        bool outOfBounds = viewPos.x < -0.1f || viewPos.x > 1.1f ||
                           viewPos.y < -0.1f || viewPos.y > 1.1f;
        if (outOfBounds)
            Destroy(gameObject);
    }
}
