using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControll : MonoBehaviour
{

    [Header("MouseSettings")]
    public bool lockCursor;
    public float mouseSensitivity = 10;
    public Transform followTarget;

    [Header("TargetDist Set")]
    public Vector2 pitchMinMax = new Vector2(-40, 85);
    public float distFromTarget = 2;
    
    public float rotationSmoothTime = .12f;
    Vector3 rotationSmoothVelocity;
    Vector3 currentRotation;

    float yaw;
    float pitch;

    // Start is called before the first frame update
    void Start()
    {
        if(lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void LateUpdate()
    {
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);

        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotationSmoothVelocity, rotationSmoothTime);
        transform.eulerAngles = currentRotation;
        Vector3 e = transform.eulerAngles;
        e.x = 0;

        transform.position = followTarget.position - transform.forward * distFromTarget;
    }
}
