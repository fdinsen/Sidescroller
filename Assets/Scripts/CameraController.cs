using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] [Range(0.0f, 1f)] private float cameraMoveSpeed = 0.2f;

    private Quaternion targetRotation = Quaternion.Euler(0,0,0);
    private Vector3 targetPosition;

    // Start is called before the first frame update
    void Start()
    {
        targetPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        DoMove();
        DoTurn();
    }

    public void MoveCamera(Vector3 target) {
        targetPosition = target;
    }

    public void RotateCamera(int angle) {
        targetRotation = Quaternion.Euler(0, angle, 0);
    }

    private void DoTurn() {
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, cameraMoveSpeed);
    }

    private void DoMove() {
        transform.position = new Vector3(
            Mathf.Lerp(transform.position.x, targetPosition.x, cameraMoveSpeed),
            Mathf.Lerp(transform.position.y, targetPosition.y, cameraMoveSpeed),
            Mathf.Lerp(transform.position.z, targetPosition.z, cameraMoveSpeed));
    }
}
