using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    private bool isMoving = false;
    private Vector3 lastPosition;
    //gravity
    [SerializeField] private float gravity;
    [SerializeField] private float jumpForceBase;
    [SerializeField] private float coyoteTime;  //set by user, max coyote time value
    [SerializeField] private float timeBetweenJumps; //max time between consecutive jumps
    private int currentJump = 0;
    private float coyoteTimer;                  //timer of coyote time
    private float groundTimer;                  //timer since ground
    private bool canLand = false;
    //camera
    [SerializeField] Camera camera;
    //cappy
    [SerializeField] GameObject cappy;
    private float forwardOffset = 1.4f;        //offset to prevent player clipping
    private float spawnHeight = 0.6f;          //giving extra height to cappy spawnpoint
    //animator manager
    Animator_Player_Manager anim;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator_Player_Manager>();
        coyoteTimer = coyoteTime;
        lastPosition = transform.position;
    }

    private void Update()
    {
        //fall off the map
        if (transform.position.y <= -1f) SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //is moving or not?
        isMoving = Input_Manager._INPUT_MANAGER.ChangeInDirection();
        anim.setMove(isMoving);

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
        if (!controller.isGrounded)
        {
            groundTimer = 0f;
        }
        else
        {
            if (canLand && groundTimer >= 0.08f)
            {
                anim.setTriggerEndJump();
                canLand = false;
            }
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

    /// <summary>
    /// Applies a jumpforce based on current consecutive jump
    /// </summary>
    private void Jump()
    {
        anim.setTriggerJump();
        anim.setJumpPhase(currentJump);
        switch(currentJump)
        {
            case 0:
                finalVelocity.y = jumpForceBase;
                currentJump = 1;
                break;
            case 1:
                finalVelocity.y = jumpForceBase * 1.25f;
                currentJump = 2;
                break;
            case 2:
                finalVelocity.y = jumpForceBase * 1.5f;
                currentJump = 0;
                break;
            default:
                finalVelocity.y = jumpForceBase;
                break;
        }
        canLand = true;
    }

    /// <summary>
    /// We call this when we want to apply an upward force to our character from an outsider
    /// </summary>
    /// <param name="value"></param>
    public void JumpSecond()
    {
        currentJump = 2;
        Jump();
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
}