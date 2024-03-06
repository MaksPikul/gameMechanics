using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{

    PlayerControls playerControls;
    PlayerMovement playerMovement;
    AnimatorManager animatorManager;
    public Vector2 movementInput;
    public Vector2 cameraInput;
    public float horizontalInput;
    public float verticalInput;
    public float cameraVertical;
    public float cameraHorizontal;
    public float moveAmount;
    public bool sprintInput;
    public bool jumpInput;

    private void Awake(){
        animatorManager = GetComponent<AnimatorManager>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void OnEnable(){
        if (playerControls == null){
            playerControls = new PlayerControls();

            playerControls.Movement.WASD.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.Actions.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();

            playerControls.Actions.Jump.performed += i => jumpInput = true;

            playerControls.Movement.Sprint.performed += i => sprintInput = true;
            playerControls.Movement.Sprint.canceled += i => sprintInput = false;
        }
        playerControls.Enable();
    }

    private void OnDisable(){
        playerControls.Disable();
    }

    public void HandleAllInputs(){
        HandleMovementInput();
        HandleSprintInput();
        HandleJumpInput();
    }

    private void HandleJumpInput(){
        if (jumpInput){

            jumpInput = false;
            playerMovement.HandleJump();
        }
    }

    private void HandleMovementInput(){
        horizontalInput = movementInput.x;
        verticalInput = movementInput.y;

        cameraHorizontal = cameraInput.y;
        cameraVertical = cameraInput.x;

        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        animatorManager.UpdateAnimatorValues(horizontalInput, verticalInput, playerMovement.isSprinting);
    }
    
    private void HandleSprintInput(){
        if (sprintInput && moveAmount > 0.5f){
            playerMovement.isSprinting = true;
        }
        else{
            playerMovement.isSprinting = false;
        }
    }
}
