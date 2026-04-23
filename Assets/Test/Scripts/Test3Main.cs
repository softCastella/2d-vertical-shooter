using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Test3Main : MonoBehaviour
{
    public Button btn;
    public Enemy3 enemyAGo;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyAGo.onDie = () => 
            {Debug.Log("enemytAgo가 죽었습니다."); };
        //람다식
        btn.onClick.AddListener(() =>
        {
            Debug.Log("clicked!");
            enemyAGo.TakeDamage(5);
            // Destroy(enemyAGo);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
