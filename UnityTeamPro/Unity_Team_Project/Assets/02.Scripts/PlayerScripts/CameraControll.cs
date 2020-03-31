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

    [Header("Zoom In/Out Camera")]
    [Tooltip("카메라 줌인 FOV 입니다.")]    public int zoomFOV = 20;
    [Tooltip("카메라 줌아웃 FOV 입니다.")]  public int normalFOV = 60;
    [Tooltip("카메라 줌인 속도입니다.")]    public float smooth = 5.0f;
    public Transform zoomTarget;

    private bool isZoom;
    private Camera camera;

    //Zoom 기능 프로퍼티.
    public bool _isZoomed
    {
        get
        {
            return isZoom;
        }
        set
        {
            isZoom = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if(lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        camera = GetComponent<Camera>();
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

        //카메라의 위치를 타겟으로부터 설정.
        transform.position = followTarget.position - transform.forward * distFromTarget;
        //Zoom 이 활성화 되어있냐에 따라 Follow타겟으로 잡을지 Zoom타겟으로 잡을지 결정.
        //transform.position = ((isZoom) ? zoomTarget.position : followTarget.position) - transform.forward * distFromTarget;

        //if(isZoom)
        //{
        //    camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, zoomFOV, Time.deltaTime * smooth);
        //}
        //else if(!isZoom)
        //{
        //    camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, normalFOV, Time.deltaTime * smooth);
        //}
    }
}
