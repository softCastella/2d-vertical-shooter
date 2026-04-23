using UnityEngine;


public class TestPlayer : MonoBehaviour
{
    private Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        int dirX = (int)Input.GetAxisRaw("Horizontal"); //a,s,d 화살표로 -1~1까지 나와 방향 조절 가능
        if (dirX == 0)
        {
            animator.SetInteger("dirX", dirX);
        }
        else if (dirX == -1)
        {
            animator.SetInteger("dirX", dirX);
            
        } else
            animator.SetInteger("dirX", dirX);
    }
}