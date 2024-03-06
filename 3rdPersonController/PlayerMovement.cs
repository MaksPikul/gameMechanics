using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    InputManager inputManager;
    Transform cameraObject;
    Rigidbody rigidBody;
    PlayerManager playerManager;
    AnimatorManager animatorManager;
    public Vector3 moveDirection;
    public LayerMask groundLayer;

    [Header("movement Status")]
    public bool isSprinting;
    public bool isGrounded;
    public bool isJumping;
    [Header("Movement Speeds")]
    public float walkSpeed = 1.5f;
    public float runSpeed = 5;
    public float sprintSpeed = 7;
    public float rotationSpeed = 10;
    public float leapingVelocity = 3;
    public float fallingSpeed = 33;
    public float airTimer;
    public float rayCastHeightOffset = 0.5f;
    [Header("Jump Stuff")]
    public float gravityIntensity = -15f;
    public float jumpHeight = 3;
    public Vector3 velo;




    private void Awake(){
        inputManager = GetComponent<InputManager>();
        rigidBody = GetComponent<Rigidbody>();
        cameraObject = Camera.main.transform;
        playerManager = GetComponent<PlayerManager>();
        animatorManager = GetComponent<AnimatorManager>();
    }
    
    public void Update(){
    velo = rigidBody.velocity;
    }

    private void HandleMovement(){

        if (isJumping){
            return;
        }

        //will move in the direction of the camera
        moveDirection = cameraObject.forward * inputManager.verticalInput;
        moveDirection = moveDirection + cameraObject.right * inputManager.horizontalInput;
        moveDirection.Normalize();
        moveDirection.y = 0;

        if (isSprinting){
            moveDirection = moveDirection * sprintSpeed;
        }
        else{
            if (inputManager.moveAmount >= 0.5f){
                moveDirection = moveDirection * runSpeed;
            }
            else{
                moveDirection = moveDirection * walkSpeed;
            }
        }

        Vector3 movementVelocity = moveDirection ;
        rigidBody.velocity = movementVelocity;
    }

    private void HandleRotation(){

        if (isJumping){
            return;
        }
        
        //code to make object face in direction of itself, not camera
        Vector3 targetDirection = Vector3.zero;
        targetDirection = cameraObject.forward * inputManager.verticalInput;
        targetDirection = targetDirection + cameraObject.right * inputManager.horizontalInput;
        targetDirection.y = 0;

        if (targetDirection == Vector3.zero){
            targetDirection = transform.forward;
        }
        Vector3 cameraForward = cameraObject.forward;
        cameraForward.y=0;
        cameraForward = cameraForward.normalized;
        
                                                            //targetDirection
        Quaternion targetRotation = Quaternion.LookRotation(cameraForward);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        transform.rotation = playerRotation;
       
    }

    public void HandleAllMovement(){

        HandleFallandLand();

        if (playerManager.doingAction){
        return;
        }
        HandleMovement();
        HandleRotation();
    }

    public void HandleFallandLand(){
        RaycastHit hit;
        Vector3 rayCastOrigin = transform.position;
        rayCastOrigin.y = rayCastOrigin.y + rayCastHeightOffset;

        if (!isGrounded && !isJumping){
            if (!playerManager.doingAction){
                animatorManager.PlayTargetAnimation("Falling", true);
            }
            airTimer = airTimer + Time.deltaTime;
            rigidBody.AddForce(transform.forward * leapingVelocity);
            rigidBody.AddForce(-Vector3.up * fallingSpeed * airTimer);
        }

        if (Physics.SphereCast(rayCastOrigin, 0.2f, -Vector3.up, out hit, groundLayer)){
            if (!isGrounded && !playerManager.doingAction){
                animatorManager.PlayTargetAnimation("Land", true);
            }
            airTimer = 0;
            isGrounded = true;
        }
        else{
            isGrounded = false;
        }
    }

    public void HandleJump(){
        if (isGrounded){

            animatorManager.animator.SetBool("isJumping", true);
            animatorManager.PlayTargetAnimation("Jump", false);

            float jumpingVelocity = Mathf.Sqrt(-2 * gravityIntensity * jumpHeight);
            Vector3 playerVelocity;
            playerVelocity = moveDirection;
            playerVelocity.y = jumpingVelocity;
            rigidBody.velocity = playerVelocity;
        }
    }
}
