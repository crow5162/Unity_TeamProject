using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCamera : MonoBehaviour
{
    [Header("Target Setting")]
    public Transform targetTransform;

    [Header("Camera Offset")]
    public float offsetX;
    public float offsetY;
    public float offsetZ;
    public float cameraRotationX = 45.0f;

    public float delayTime;
    private void Start()
    {
        transform.eulerAngles = new Vector3(cameraRotationX, 0, 0);
    }

    private void LateUpdate()
    {
        Vector3 newPos = new Vector3(targetTransform.position.x + offsetX,
                                      targetTransform.position.y + offsetY,
                                      targetTransform.position.z + offsetZ);

        transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * delayTime);
    }
}
