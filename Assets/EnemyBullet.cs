using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private float speed = 8f; // 총알 이동 속도
    public int damage = 10;
    [HideInInspector]
    public Vector3 moveDirection = Vector3.down; // Enemy에서 발사 시 방향 설정

    void Update()
    {
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
    }

    // 플레이어에 닿으면 데미지를 주고 총알 제거
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerCont player = other.GetComponent<PlayerCont>();
            if (player != null) player.TakeDamage(damage);
            
            Debug.Log("Player HP: " + player.hp);
           
            Destroy(gameObject);
        }
    }
}
