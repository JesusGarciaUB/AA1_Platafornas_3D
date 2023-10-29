using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animator_Player_Manager : MonoBehaviour
{
    private Animator animator;
    private PlayerMovement player;
    private CharacterController controller;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        player = GetComponent<PlayerMovement>();
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        animator.SetBool("moving", Input_Manager._INPUT_MANAGER.ChangeInDirection());
        animator.SetBool("jumping", !controller.isGrounded);
        animator.SetInteger("jumpPhase", player.GetCurrentJumpPhase());
        Debug.Log(Input_Manager._INPUT_MANAGER.ChangeInDirection());
        Debug.Log(!controller.isGrounded);
        Debug.Log(player.GetCurrentJumpPhase());
    }
}
