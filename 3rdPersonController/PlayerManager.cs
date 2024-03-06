using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.EventSystems;

public class PlayerManager : MonoBehaviour
{
    InputManager inputManager;
    PlayerMovement playerMovement;
    CameraManager cameraManager;
    Animator animator;
    public bool doingAction;


    public void Awake(){

        inputManager = GetComponent<InputManager>();
        playerMovement = GetComponent<PlayerMovement>();
        cameraManager = FindObjectOfType<CameraManager>();
        animator = GetComponent<Animator>();
        
    }

    private void Update(){
        inputManager.HandleAllInputs();
    }

    private void FixedUpdate(){
        playerMovement.HandleAllMovement();
    }

    private void LateUpdate(){
        cameraManager.HandleAllCamera();

        doingAction = animator.GetBool("doingAction");
        playerMovement.isJumping = animator.GetBool("isJumping");
        animator.SetBool("isGrounded", playerMovement.isGrounded);
        
    }


}
