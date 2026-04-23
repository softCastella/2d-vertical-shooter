using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Test2 : MonoBehaviour
{
    public Button btn;

    public SpriteRenderer sr;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(FadeOut());
    }

    private int i;

    // Update is called once per frame
    IEnumerator FadeOut()
    {
        for (i = 0; i <= 255; i++)
        {
            var newAlpha = 1 - (i / 255f);
            sr.color = new Color(1, 1, 1, newAlpha);
            i++;
            yield return null;
        }
    }
}