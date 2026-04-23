using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Test3Main : MonoBehaviour
{
    public Button btn;
    public Enemy3 enemyAGo;
    public GameObject[] itemPrefabs;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Vector3 pos = enemyAGo.pos;
        enemyAGo.onDie = () => 
            {Debug.Log("enemytAgo가 죽었습니다.");
            ItemManager.Instance.CreateItem(pos); };
        //람다식
        btn.onClick.AddListener(() =>
        {
            Debug.Log("clicked!");
            enemyAGo.TakeDamage(5);
            Debug.Log("enemyAGo의 HP: " + enemyAGo.hp);     
            Destroy(enemyAGo.gameObject);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
