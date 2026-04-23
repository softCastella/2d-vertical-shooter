using UnityEngine;
using System;


public class Enemy3_2 : MonoBehaviour
{
    public Vector3 pos;
    public Action onDie;
    public int hp;
    public int maxHp = 10;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //위치 설정
        pos = transform.position;
        //체력 설정
        hp = maxHp;
    }

    //데미지 메서드
    public void TakeDamage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            onDie();
            Destroy(gameObject);
            
        }
    }




    // Update is called once per frame
    void Update()
    {
        
    }
}
