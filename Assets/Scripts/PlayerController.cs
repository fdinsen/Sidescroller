using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Serialized Variables
    [SerializeField] private float speed = 1;
    [SerializeField] private float jumpForce = 1;
    [SerializeField] private CameraController cam;
    [SerializeField] private float secondChanceTime = 1f;

    private Rigidbody rigidbody;

    //Movement Variables
    private bool isOnGround = false;
    private bool directionIsX = true;
    private int currentRotation = 0;
    private float secondChanceCooldown = 0;

    //Raycasting Variables
    Vector3 dwn;
    readonly int layerMask = 1 << 3;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        dwn = transform.TransformDirection(Vector3.down);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        CheckGround();
    }

    private void OnTriggerEnter(Collider other) {
        MoveToNextArea(other);
    }

    private void Move() {
        Jump();

        //For each direction the player can move in, do move
        if (currentRotation == 0) {
            float h = Input.GetAxisRaw("Horizontal");
            gameObject.transform.position = new Vector3(transform.position.x + (h * speed) * Time.deltaTime,
               transform.position.y, transform.position.z);
        } else if (currentRotation == -90) {
            float h = Input.GetAxisRaw("Horizontal");
            gameObject.transform.position = new Vector3(transform.position.x,
               transform.position.y, transform.position.z + (h * speed) * Time.deltaTime);
        } else if (currentRotation == 90) {
            float h = Input.GetAxisRaw("Horizontal") * -1;
            gameObject.transform.position = new Vector3(transform.position.x,
               transform.position.y, transform.position.z + (h * speed) * Time.deltaTime);
        } else if (currentRotation == 180) {
            float h = Input.GetAxisRaw("Horizontal") * -1;
            gameObject.transform.position = new Vector3(transform.position.x + (h * speed) * Time.deltaTime,
               transform.position.y, transform.position.z);
        }
    }

    private void Jump() {
        if (isOnGround) {
            //Reset cooldown timer if player is on ground
            secondChanceCooldown = secondChanceTime;
        }
        if (secondChanceCooldown > 0) {
            //Count down timer for second chance to jump
            secondChanceCooldown -= Time.deltaTime;

            //DoJump
            if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))) {
                rigidbody.AddForce(new Vector3(0, jumpForce, 0));
            }
        }
    }

    private void MoveToNextArea(Collider other) {
        if (other.gameObject.CompareTag("Area")) {

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

    private void CheckGround() {
        //Casts a ray down 1.2 unit to check if the player is grounded
        isOnGround = Physics.Raycast(transform.position, dwn, 1.2f, layerMask);
    }

}
