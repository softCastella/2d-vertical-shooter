using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemManager3 : MonoBehaviour
{
    public GameObject[] items;

    private void Awake()
    {
        Instance = this;
    }
    

    public void CreateItem(Vector3 pos)
    {
        //아이템을 만든다
        var prefab = items[Random.Range(0, items.Length)];
        var go = Instantiate(prefab, pos, Quaternion.identity);
    }

    
}
