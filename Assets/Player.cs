using UnityEngine;

public class Player : MonoBehaviour
{
    //외부에서 안으로 할당 한다
    public Transform firePoint;
    public GameObject PlayerBulletPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log($"firePoint{firePoint}");
        Debug.Log(firePoint.position);
        Debug.Log($"PlayerBulletPrefab: {PlayerBulletPrefab}");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //프리펩 인스턴스를 게임오브젝트 가변수에 넣고
            GameObject playerBulletGo = Instantiate(PlayerBulletPrefab);
            Debug.Log(playerBulletGo.transform.position);
            //
            playerBulletGo.transform.position = firePoint.position;
            Debug.Log(playerBulletGo.transform.position);
        }
    }
}