using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform cameraTransform;
    public Rigidbody rb = new Rigidbody();

    public float moveSpeed = 5.0f;
    public float runMultiplier = 0.5f;
    public float jumpMultiplier = 5.0f;
    public float rotationSpeed = 10.0f;

    float horizontal;
    float vertical;
    float jump;
    float run;
    float select;

    bool grounded = false;

    public void FixedUpdate()
    {
        Vector3 moveDirection = Vector3.forward * vertical + Vector3.right * horizontal;
        Vector3 projectedCameraForward = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up);
        Quaternion rotationToCamera = Quaternion.LookRotation(projectedCameraForward, Vector3.up);
        
        moveDirection = rotationToCamera.normalized * moveDirection;

        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationToCamera, rotationSpeed * Time.deltaTime);
        
        transform.position += moveDirection * moveSpeed * (run * runMultiplier + 1) * Time.deltaTime;

        if (grounded) {
            rb.AddForce(Vector3.up * jump, ForceMode.Impulse);
        }
        else
        {
            rb.AddForce(Physics.gravity * rb.mass);
        }
    }
    
    public void OnMoveInput(float horizontal, float vertical)
    {
        this.vertical = vertical;
        this.horizontal = horizontal;
    }

    public void OnJumpInput(float jump)
    {
        this.jump = jump * jumpMultiplier;
    }

    public void OnRunInput(float run)
    {
        this.run = run;
    }
    
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            grounded = true;
        }
        Debug.Log(grounded);
    }

    public void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            grounded = false;
        }
        Debug.Log(grounded);
    }
}
