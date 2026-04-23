using UnityEngine;
using UnityEngine.UI;

public class Test3Main_2 : MonoBehaviour
{
    public Button btn;
    public Enemy3_2 enemy3_2;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //적이 죽었음을 Enemy스크립트 대신 알려줄 대리자
        enemy3_2.onDie = () =>
        {
            Debug.Log("적이 죽었습니다.");
            ItemManager.instance.DropRandom(enemy3_2.pos);
        };
        //버튼을 눌렀을때 알려줌
        btn.onClick.AddListener(() =>
        {
            Debug.Log("Attack했습니다.");
            enemy3_2.TakeDamage(5);
            Debug.Log($"적의 HP:{enemy3_2.hp}");
            
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
