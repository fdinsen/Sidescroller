using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 1;
    [SerializeField] private float jumpForce = 1;
    [SerializeField] private CameraController cam;

    private Rigidbody rigidbody;

    private bool isOnGround = false;
    private bool directionIsX = true;
    private int currentRotation = 0;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void OnCollisionStay(Collision collision) {
        if (collision.collider.gameObject.CompareTag("Ground")) {
            isOnGround = true;
        }
    }

    private void OnCollisionExit(Collision collision) {
        if (collision.collider.gameObject.CompareTag("Ground")) {
            isOnGround = false;
        }
    }

    private void OnTriggerEnter(Collider other) {
        MoveToNextArea(other);
    }

    private void Move() {
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) && isOnGround) {
            rigidbody.AddForce(new Vector3(0, jumpForce, 0));
        }

        if (currentRotation == 0) {
            float h = Input.GetAxisRaw("Horizontal");
            gameObject.transform.position = new Vector3(transform.position.x + (h * speed) * Time.deltaTime,
               transform.position.y, transform.position.z);
        } else if(currentRotation == -90) {
            float h = Input.GetAxisRaw("Horizontal");
            gameObject.transform.position = new Vector3(transform.position.x,
               transform.position.y, transform.position.z + (h * speed) * Time.deltaTime);
        } else if(currentRotation == 90) {
            float h = Input.GetAxisRaw("Horizontal") * - 1;
            gameObject.transform.position = new Vector3(transform.position.x,
               transform.position.y, transform.position.z + (h * speed) * Time.deltaTime);
        } else if(currentRotation == 180) {
            float h = Input.GetAxisRaw("Horizontal") * -1;
            gameObject.transform.position = new Vector3(transform.position.x + (h * speed) * Time.deltaTime,
               transform.position.y, transform.position.z);
        }
    }

    private void MoveToNextArea(Collider other) {
        if (other.gameObject.tag.Equals("Area")) {

            AreaInfo info = other.GetComponent<AreaInfo>();
            Vector3 targetPosition = info.GetCameraPosition();
            int targetRotation = info.GetCameraRotation();

            cam.MoveCamera(targetPosition);
            cam.RotateCamera(targetRotation);

            if (targetRotation != currentRotation) {
                if (directionIsX) {
                    gameObject.transform.position
                        = new Vector3(other.gameObject.transform.position.x, transform.position.y, transform.position.z);
                } else {
                    gameObject.transform.position
                        = new Vector3(transform.position.x, transform.position.y, other.gameObject.transform.position.z);
                }

                directionIsX = !directionIsX;
                currentRotation = targetRotation;
            }
        }
    }
}
