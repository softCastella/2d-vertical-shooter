using UnityEngine;

public class Item3 : MonoBehaviour
{
    public enum ItemType
    {
        None = -1, Coin,Boom, Power
    }

    public ItemType itemType = ItemType.None;

    void Start()
    {
        // Debug.Log($"{gameObject.name},{itemType}");
    }

    void Update()
    {
        
    }
}
