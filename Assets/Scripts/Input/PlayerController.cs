using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputController _inputController = default;

    public Transform cameraTransform;
    public Rigidbody rb = new Rigidbody();

    public float moveSpeed = 5.0f;
    public float jumpMultiplier = 5.0f;
    public float rotationSpeed = 10.0f;

    Vector2 movement;
    float jump;
    bool runInput;
    float select;

    Vector3 lookDirection;
    Vector3 moveDirection;
    Vector3 projectedCameraForward;
    Quaternion rotationToCamera;

    bool grounded = false;

    public void Start()
    {
        lookDirection = transform.forward;
    }

    public void OnEnable()
    {
        _inputController.moveInputEvent += OnMoveInput;
        _inputController.jumpInputEvent += OnJumpInitiated;
    }

    public void OnDisable()
    {
        _inputController.moveInputEvent -= OnMoveInput;
        _inputController.jumpInputEvent -= OnJumpInitiated;
    }

    public void FixedUpdate()
    {
        projectedCameraForward = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up);
        rotationToCamera = Quaternion.LookRotation(projectedCameraForward, Vector3.up);

        moveDirection = Vector3.forward * movement.y + Vector3.right * movement.x;
        moveDirection = rotationToCamera.normalized * moveDirection;
        if (moveDirection != Vector3.zero)
            lookDirection = moveDirection;

        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(lookDirection), rotationSpeed * Time.deltaTime);

        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        if (grounded) {
            rb.AddForce(Vector3.up * jump, ForceMode.Impulse);
        }
        else
        {
            rb.AddForce(Physics.gravity * rb.mass);
        }
    }
    
    public void OnMoveInput(Vector2 direction)
    {
        this.movement = direction;
    }

    public void OnJumpInitiated(float jumped)
    {
        this.jump = jumpMultiplier * jumped;
    }
    
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            grounded = true;
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            grounded = false;
        }
    }
}
