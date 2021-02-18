using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaInfo : MonoBehaviour
{
    [SerializeField] private Vector3 cameraPosition;
    [SerializeField] private int cameraRotation = 0;
    
    public int GetCameraRotation() {
        return cameraRotation;
    }

    public Vector3 GetCameraPosition() {
        return cameraPosition;
    }
}
