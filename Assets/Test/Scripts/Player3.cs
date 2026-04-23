using System;
using UnityEngine;

public class Player3 : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        var item3 = other.GetComponent<Item3>();
        Debug.Log(item3.itemType);
        //     Destroy(other.gameObject);
        // }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
