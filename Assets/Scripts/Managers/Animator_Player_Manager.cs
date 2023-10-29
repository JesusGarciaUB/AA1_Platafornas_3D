using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animator_Player_Manager : MonoBehaviour
{
    private Animator animator;
    private PlayerMovement player;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        player = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        animator.SetBool("moving", Input_Manager._INPUT_MANAGER.ChangeInDirection());
        animator.SetBool("jumping", player.GetIsGrounded());
        animator.SetInteger("jumpPhase", player.GetCurrentJumpPhase());
    }
}
