using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Serialized variables
    [SerializeField] private float movementSpeeed = 10;
    [SerializeField] private CameraController cameraController;

    private Rigidbody rb;

    // Jump related
    private float _jumpLastTime;
    [SerializeField] private float jumpTimeDisabled = 1;
    [SerializeField] private float jumpForce = 300;

    // Movement related
    private int currentPlayerRot = 0;
    private bool directionIsX = true;

    // IsOnGround related
    private readonly int _mask = 1 << 3;

    // Called before the first frame
    private void Start() {
        rb = GetComponent<Rigidbody>();
        _jumpLastTime = Time.time;
    }

    // Called once per frame
    private void Update() {
        Jump();
        Movement();
    }

    // Called based on the trigger
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Area")) {
            MoveToNextArea(other);
        }
    }

    // Handles the camera POV
    private void MoveToNextArea(Collider other) {
        AreaInfo areaInfo = other.GetComponent<AreaInfo>();
        Vector3 targetCamPos = areaInfo.GetCameraPosition();
        int targetCamRot = areaInfo.GetCameraRotation();

        cameraController.MoveCamera(targetCamPos);
        cameraController.RotateCamera(targetCamRot);

        if(currentPlayerRot != targetCamRot) {
            Vector3 newPos;

            if(directionIsX) {
                newPos = new Vector3(other.gameObject.transform.position.x, transform.position.y, transform.position.z);
            } else {
                newPos = new Vector3(transform.position.x, transform.position.y, other.gameObject.transform.position.z);
            }

            gameObject.transform.position = newPos;

            directionIsX = !directionIsX;
            currentPlayerRot = targetCamRot;
        }
    }

    // Handles the players ability to jump
    private void Jump() {
        bool jumpRequested = Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow);
        bool allowedByTimer = Time.time - _jumpLastTime > jumpTimeDisabled;
        
        if(jumpRequested && IsOnGround() && allowedByTimer) {
            _jumpLastTime = Time.time;
            rb.AddForce(new Vector3(0, jumpForce, 0));
        }
    }

    private bool IsOnGround() {
        return Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), 1.1f, _mask);
    }

    // Handles the players ability to move
    private void Movement() {
        float horizontal = Input.GetAxisRaw("Horizontal");

        if(currentPlayerRot == 90 || currentPlayerRot == 180) {
            horizontal *= -1;
        }

        if(currentPlayerRot == 0 || currentPlayerRot == 180) {
            gameObject.transform.position = new Vector3(transform.position.x + (horizontal * movementSpeeed) * Time.deltaTime, transform.position.y, transform.position.z);
        } else if(currentPlayerRot == -90 || currentPlayerRot == 90) {
            gameObject.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + (horizontal * movementSpeeed) * Time.deltaTime);
        } else {
            Debug.Log("Player Movement Error: Unknown player rotation: " + horizontal);
        }
    }

}