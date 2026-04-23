using System;
using UnityEngine;

public class Enemy3 : MonoBehaviour
{
    public int hp;
    public int maxHp;
    public Action onDie;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hp = maxHp;
        
    }

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
