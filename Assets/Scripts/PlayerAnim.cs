using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    public void Jump()
    {
        animator.SetTrigger("Jump");
    }

    public void Idle()
    {
        animator.SetBool("IsWalking", false);
        animator.SetBool("IsRunning", false);
    }

    public void Walk()
    {
        animator.SetBool("IsWalking", true);
        animator.SetBool("IsRunning", false);
    }

    public void Run()
    {
        animator.SetBool("IsWalking", false);
        animator.SetBool("IsRunning", true);
    }

    public void Attack()
    {
        animator.SetTrigger("Attack");
    }
}