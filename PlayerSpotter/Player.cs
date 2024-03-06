using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 7;
    public float smoothMoveTime = 0.1f;
    public float turnSpeed = 8;
    Rigidbody rigidbody;

    float angle;
    float smoothInputMag;
    float smoothMoveVelocity;
    Vector3 Velocity;

    void Start(){
        rigidbody = GetComponent<Rigidbody>();
    }

    void Update(){
        Vector3 inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"),0, Input.GetAxisRaw("Vertical")).normalized;
        float inputMag = inputDirection.magnitude;
        smoothInputMag = Mathf.SmoothDamp(smoothInputMag, inputMag, ref smoothMoveVelocity, smoothMoveTime);

        float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;
        angle = Mathf.LerpAngle(angle, targetAngle, Time.deltaTime * turnSpeed * inputMag);
        transform.eulerAngles = Vector3.up * angle;

        transform.Translate(transform.forward * moveSpeed * Time.deltaTime * inputMag, Space.World);
        Velocity = transform.forward * moveSpeed * smoothInputMag;
    }

    private void FixedUpdate() {
        rigidbody.MoveRotation(Quaternion.Euler(Vector3.up * angle));
        rigidbody.MovePosition(rigidbody.position + Velocity * Time.deltaTime);
    }

    

}
