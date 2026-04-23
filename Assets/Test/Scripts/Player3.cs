using System;
using UnityEngine;

public class Player3 : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        var item3 = other.gameObject.GetComponent<Item3>();
        Debug.Log(item3.itemType);
        
    }
}
