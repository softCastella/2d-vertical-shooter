using UnityEngine;
using System.Collections;

public class Item3 : MonoBehaviour
{
    public enum ItemType
    {
        None = -1, Coin,Boom, Power
    }

    public ItemType itemType = ItemType.None;
    public float speed = 1f;
    
    public IEnumerator Move()
    {   while (true)
        {
            transform.Translate(Vector3.down *speed* Time.deltaTime);
            yield return null; //다음 프레임으로 넘김

            if (transform.position.y < -5.5)
            break;
        }
        Destroy(gameObject);
    }
}
