using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    InputManager inputManager;
    public Transform targetTransform; //object camera will follow
    public Transform cameraPivot;
    public Transform cameraTransform;
    public LayerMask collisionLayers;
    private float defaultPosition;
    private Vector3 cameraVectorPosition;
    private float cameraCollisionRadius = 0.2f;
    //follow speeds
    private Vector3 cameraFollowVelocity = Vector3.zero;
    public float cameraFollowSpeed = 0.1f;
    //rotate speeds
    public float lookAngle;
    public float cameraLookSpeed = 2;
    public float cameraPivotSpeed = 2;
    public float pivotAngle;
    public float minAngle = -25;
    public float maxAngle = 25;

    public float cameraCollisionOffset = 0.2f;
    public float minCollisionOffset = 0.2f;


    private void Awake(){
        inputManager = FindObjectOfType<InputManager>();
        targetTransform = FindObjectOfType<PlayerManager>().transform;
        cameraTransform = Camera.main.transform;
        defaultPosition = cameraTransform.localPosition.z;
        
    }
    public void HandleAllCamera(){
        FollowTarget();
        RotateCamera();
        HandleCameraCollisions();
    }

    private void FollowTarget(){
        Vector3 targetPosition = Vector3.SmoothDamp
            (transform.position, targetTransform.position, ref cameraFollowVelocity, cameraFollowSpeed);

        transform.position = targetPosition;
    }

    private void RotateCamera(){
        lookAngle = lookAngle + (inputManager.cameraVertical * cameraLookSpeed);
        pivotAngle = pivotAngle - (inputManager.cameraHorizontal * cameraPivotSpeed);
        pivotAngle = Mathf.Clamp(pivotAngle, minAngle, maxAngle);

        Vector3 rotation = Vector3.zero;
        rotation.y = lookAngle;
        Quaternion targetRotation = Quaternion.Euler(rotation);
        transform.rotation = targetRotation;

        rotation = Vector3.zero;
        rotation.x = pivotAngle;
        targetRotation = Quaternion.Euler(rotation);
        cameraPivot.localRotation = targetRotation;
    }

    private void HandleCameraCollisions(){
        float targetPosition = defaultPosition;
        //stores what camera hit
        RaycastHit hit;
        Vector3 direction = cameraTransform.position - cameraPivot.position;
        direction.Normalize();
        
        if (Physics.SphereCast
            (cameraPivot.transform.position, cameraCollisionRadius, direction, out hit, Mathf.Abs(targetPosition),collisionLayers)){
            
            float distance = Vector3.Distance(cameraPivot.position, hit.point);
            targetPosition = targetPosition - (distance - cameraCollisionOffset);
        }

        if (Mathf.Abs(targetPosition) < minCollisionOffset){
            targetPosition = targetPosition -minCollisionOffset;
        }

        cameraVectorPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, 0.2f);
        cameraTransform.localPosition = cameraVectorPosition;
    }
}
