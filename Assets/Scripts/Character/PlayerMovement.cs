using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //movement
    private CharacterController controller;
    private Vector3 finalVelocity = Vector3.zero;
    private float velocityXZ = 5f;
    private Vector3 dir;
    //gravity
    [SerializeField] private float gravity;
    [SerializeField] private float jumpForce;
    [SerializeField] private float coyoteTime;  //set by user, max coyote time value
    [SerializeField] private float timeBetweenJumps; //max time between consecutive jumps
    private int currentJump;
    private float coyoteTimer;                  //timer of coyote time
    private float groundTimer;                  //timer since ground
    //camera
    [SerializeField] Camera camera;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        coyoteTimer = coyoteTime;
    }

    private void Update()
    {
        //ground timer
        if (!controller.isGrounded) groundTimer = 0f;
        else groundTimer += Time.deltaTime;

        if (groundTimer > timeBetweenJumps) currentJump = 0;

        //direction
        dir = Quaternion.Euler(0f, camera.transform.eulerAngles.y, 0f) * new Vector3(Input_Manager._INPUT_MANAGER.GetLeftAxisValue().x, 0f, Input_Manager._INPUT_MANAGER.GetLeftAxisValue().y);

        //velocity
        finalVelocity.x = dir.x * velocityXZ;
        finalVelocity.z = dir.z * velocityXZ;

        //gravity
        dir.y = -1f;
        GravityBehaviour();

        //rotation
        if (Input_Manager._INPUT_MANAGER.ChangeInDirection())
        {
            dir.y = 0f;
            transform.rotation = Quaternion.LookRotation(dir);
        }

        //move
        controller.Move(finalVelocity * Time.deltaTime);
    }

    private void GravityBehaviour()
    {
        if (controller.isGrounded)
        {
            if (Input_Manager._INPUT_MANAGER.GetJumpButtonPressed())
            {
                Jump();
                coyoteTimer = 0f;
            }
            else
            {
                finalVelocity.y = dir.y * gravity * Time.deltaTime;
                coyoteTimer = coyoteTime;
            }
        }
        else 
        { 
            finalVelocity.y += dir.y * gravity * Time.deltaTime;
            coyoteTimer -= Time.deltaTime;

            if (Input_Manager._INPUT_MANAGER.GetJumpButtonPressed() && coyoteTimer > 0f)
            {
                Jump();
                coyoteTimer = 0f;
            }
        }
    }

    private void Jump()
    {
        switch(currentJump)
        {
            case 0:
                finalVelocity.y = jumpForce;
                currentJump++;
                break;
            case 1:
                finalVelocity.y = jumpForce * 1.25f;
                currentJump++;
                break;
            case 2:
                finalVelocity.y = jumpForce * 1.5f;
                currentJump = 0;
                break;
        }
    }
}