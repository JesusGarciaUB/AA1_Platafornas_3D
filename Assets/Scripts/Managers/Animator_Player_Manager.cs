using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animator_Player_Manager : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void setJumpPhase(int val)
    {
        animator.SetInteger("jumpPhase", val);
    }

    public void setMove(bool val)
    {
        animator.SetBool("moving", val);
    }

    public void setTriggerJump()
    {
        animator.SetTrigger("jump");
    }

    public void setTriggerEndJump()
    {
        animator.SetTrigger("endjump");
    }
}
