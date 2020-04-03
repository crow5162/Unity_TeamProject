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

        //offset = new Vector3(targetTransform.position.x, targetTransform.position.y + 8.0f, targetTransform.position.z + 7.0f);
        //
        //Vector3 newPos = new Vector3(targetTransform.position.x + offsetX,
        //                              targetTransform.position.y + offsetY,
        //                              targetTransform.position.z + offsetZ);
        //
        ////transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * delayTime);
        //
        //transform.position = newPos;
    }

    private void Update()
    {

        Vector3 newPos = new Vector3(targetTransform.position.x + offsetX,
                                      targetTransform.position.y + offsetY,
                                      targetTransform.position.z + offsetZ);

        transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * delayTime);

        //if(Input.GetKey(KeyCode.Q))
        //{
        //    this.transform.RotateAround(targetTransform.position, Vector3.up, 90.0f * Time.deltaTime);
        //}
        //
        //else if (Input.GetKey(KeyCode.E))
        //{
        //    this.transform.RotateAround(targetTransform.position, Vector3.down, 90.0f * Time.deltaTime);
        //}

        //if(Input.GetMouseButton(1))
        //{
        //    Vector3 newPos = new Vector3(targetTransform.position.x + offsetX,
        //                                  targetTransform.position.y + offsetY,
        //                                  targetTransform.position.z + offsetZ);
        //
        //    transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * delayTime);
        //}


        //this.transform.RotateAround(targetTransform.position, Vector3.up, 90.0f * Time.deltaTime);

    }

    private void LateUpdate()
    {


        //if(Input.GetKey(KeyCode.Q))
        //{
        //    transform.rotation *= Quaternion.AngleAxis(45 * Time.deltaTime, Vector3.up);
        //
        //    transform.position += (transform.position * );
        //}
        //
        //else if (Input.GetKey(KeyCode.E))
        //{
        //    transform.rotation *= Quaternion.AngleAxis(45 * Time.deltaTime, Vector3.down);
        //}
        
        
        //offset = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * turnSpeed, Vector3.up) * offset;
        //transform.position = targetTransform.position + offset;
        //transform.LookAt(targetTransform.position);

        //transform.position = targetTransform.position;

    }
}
