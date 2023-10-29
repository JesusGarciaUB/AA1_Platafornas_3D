using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //movement
    private CharacterController controller;
    private Vector3 finalVelocity = Vector3.zero;
    private float velocityXZ = 5f;
    private float acceleration = 8f;
    private float currentVelocity = 0f;
    private Vector3 dir;
    private Vector3 lastDir;
    private bool isCrouched = false;
    //gravity
    [SerializeField] private float gravity;
    [SerializeField] private float jumpForceBase;
    [SerializeField] private float coyoteTime;  //set by user, max coyote time value
    [SerializeField] private float timeBetweenJumps; //max time between consecutive jumps
    private int currentJump = 0;
    private float coyoteTimer;                  //timer of coyote time
    private float groundTimer;                  //timer since ground
    private bool isGrounded;                    //raycasted isgrounded, much better than buggy isgrounded from character controller
    //camera
    [SerializeField] Camera camera;
    //cappy
    [SerializeField] GameObject cappy;
    private float forwardOffset = 1.4f;        //offset to prevent player clipping
    private float spawnHeight = 0.6f;          //giving extra height to cappy spawnpoint

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        coyoteTimer = coyoteTime;
    }

    private void Update()
    {
        //raycast for better ground control
        RaycastHit hit;
        Ray landingRay = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(landingRay, out hit, 0.05f))
        {
            if (hit.collider == null) isGrounded = false;
            else isGrounded = true;
        }

        //cappy
        if (Input_Manager._INPUT_MANAGER.GetCappyPressed())
        {
            SpawnCappy();
        }

        //crouch
        if (Input_Manager._INPUT_MANAGER.GetCrouchButtonPressed())
        {
            isCrouched = !isCrouched;
        }

        //ground timer
        if (!isGrounded)
        {
            groundTimer = 0f;
        }
        else
        {
            groundTimer += Time.deltaTime;
        }

        if (groundTimer > timeBetweenJumps)
        {
            currentJump = 0;
        }

        //direction
        if (Input_Manager._INPUT_MANAGER.ChangeInDirection()) lastDir = dir;
        dir = Quaternion.Euler(0f, camera.transform.eulerAngles.y, 0f) * new Vector3(Input_Manager._INPUT_MANAGER.GetLeftAxisValue().x, 0f, Input_Manager._INPUT_MANAGER.GetLeftAxisValue().y);
        dir.Normalize();

        //velocity and acceleration
        if (Input_Manager._INPUT_MANAGER.ChangeInDirection()) currentVelocity += acceleration * Time.deltaTime;
        else currentVelocity -= acceleration * Time.deltaTime;

        if (isCrouched) currentVelocity = Mathf.Clamp(currentVelocity, 0f, velocityXZ * 0.5f);
        else currentVelocity = Mathf.Clamp(currentVelocity, 0f, velocityXZ);

        //apply velocity
        if (Input_Manager._INPUT_MANAGER.ChangeInDirection()) ApplyVelocity(dir);
        else ApplyVelocity(lastDir);    //apply velocity from last valid input, needed for deceleration

        //gravity and jump
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

    /// <summary>
    /// Applies gravity and manages jump restrictions
    /// </summary>
    private void GravityBehaviour()
    {
        if (isGrounded)
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

    /// <summary>
    /// Applies a jumpforce based on current consecutive jump
    /// </summary>
    private void Jump()
    {
        isGrounded = false;
        switch(currentJump)
        {
            case 0:
                Debug.Log("jump1");
                finalVelocity.y = jumpForceBase;
                currentJump++;
                break;
            case 1:
                Debug.Log("jump2");
                finalVelocity.y = jumpForceBase * 1.25f;
                currentJump++;
                break;
            case 2:
                Debug.Log("jump3");
                finalVelocity.y = jumpForceBase * 1.5f;
                currentJump = 0;
                break;
            default:
                Debug.Log("default");
                finalVelocity.y = jumpForceBase;
                break;
        }
    }

    /// <summary>
    /// We call this when we want to apply an upward force to our character from an outsider
    /// </summary>
    /// <param name="value"></param>
    public void Jump(float value)
    {
        finalVelocity.y = value;
        coyoteTimer = 0;
    }

    /// <summary>
    /// Applies a velocity given a direction
    /// </summary>
    /// <param name="dir"></param>
    private void ApplyVelocity(Vector3 value)
    {
        finalVelocity.x = value.x * currentVelocity;
        finalVelocity.z = value.z * currentVelocity;
    }

    /// <summary>
    /// Spawns cappy and tells the level manager, if cappy is still on scene does nothing
    /// </summary>
    private void SpawnCappy()
    {
        if (!Level_Manager._LEVEL_MANAGER.getCappySituation())
        {
            Vector3 toSpawn = transform.position + transform.forward * forwardOffset;
            toSpawn.y += spawnHeight;
            Instantiate(cappy, toSpawn, transform.rotation);
            Level_Manager._LEVEL_MANAGER.setCappySituation(true);
        }
    }

    public int GetCurrentJumpPhase()
    {
        return this.currentJump;
    }

    public bool GetIsGrounded()
    {
        return this.isGrounded;
    }
}