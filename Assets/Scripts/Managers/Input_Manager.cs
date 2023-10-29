using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Input_Manager : MonoBehaviour
{
    //Singleton
    private PlayerInput playerInputs;
    public static Input_Manager _INPUT_MANAGER;

    //Timers
    private float timeSinceJumpPressed = 0f;
    private float timeSinceCrouchPressed = 0f;

    //Values
    private Vector2 leftAxisValue = Vector2.zero;
    private Vector2 rightAxisValue = Vector2.zero;

    private void Awake()
    {
        if (_INPUT_MANAGER != null && _INPUT_MANAGER != this) Destroy(gameObject);
        else
        {
            playerInputs = new PlayerInput();
            playerInputs.Character.Enable();

            playerInputs.Character.Jump.performed += JumpButtonPressed;
            playerInputs.Character.Move.performed += LeftAxisUpdate;
            playerInputs.Character.Crouch.performed += CrouchButtonPressed;
            playerInputs.Character.CameraMove.performed += RightAxisUpdate;

            _INPUT_MANAGER = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Update()
    {
        //Timers
        timeSinceJumpPressed += Time.deltaTime;
        timeSinceCrouchPressed += Time.deltaTime;

        InputSystem.Update();
    }

    private void JumpButtonPressed(InputAction.CallbackContext context)
    {
        timeSinceJumpPressed = 0f;
    }

    private void LeftAxisUpdate(InputAction.CallbackContext context)
    {
        leftAxisValue = context.ReadValue<Vector2>();
    }

    private void CrouchButtonPressed(InputAction.CallbackContext context)
    {
        timeSinceCrouchPressed = 0f;
    }

    private void RightAxisUpdate(InputAction.CallbackContext context)
    {
        rightAxisValue = context.ReadValue<Vector2>();
    }

    //Getters

    public bool GetJumpButtonPressed()
    {
        return this.timeSinceJumpPressed == 0f;
    }

    public Vector2 GetLeftAxisValue()
    {
        return this.leftAxisValue;
    }

    public bool GetCrouchButtonPressed()
    {
        return this.timeSinceCrouchPressed == 0f;
    }

    public Vector2 GetRightAxisValue()
    {
        return this.rightAxisValue;
    }

    public bool ChangeInDirection()
    {
        return leftAxisValue.x >= 0.1f || leftAxisValue.y >= 0.1f || leftAxisValue.x <= -0.1f || leftAxisValue.y <= -0.1f;
    }
}
