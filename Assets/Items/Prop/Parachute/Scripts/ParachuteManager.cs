using UnityEngine;

public class ParachuteManager : MonoBehaviour
{
    public Animator animator;

    public void Open()
    {
        animator.SetTrigger("Open");
    }

    public void Close()
    {
        animator.SetTrigger("Close");
    }
}
