using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControll : MonoBehaviour
{
    [Header("Character Speed")]
    public float walkSpeed = 2.0f;
    public float runSpeed = 6.0f;
    public float rotSpeed = 80.0f;

    private float r = 0.0f;

    public float turnSmoothTime = 0.2f;
    public float speedSmoothTime = 0.1f;

    float turnSmoothVelocity;
    float speedSmoothVelocity; 

    float currentSpeed;

    Transform cameraTr;

    private Animator anim;
    private Transform playerTr;
    private CameraControll cControll;

    private readonly int hashMoving = Animator.StringToHash("isMove");
    private readonly int hashAiming = Animator.StringToHash("isAiming");
    private readonly int hashV = Animator.StringToHash("v");
    private readonly int hashH = Animator.StringToHash("h");

    [Header("Aiming Target")]
    private bool isAiming = false;  //조준모드
    private Transform followTr;

    // Start is called before the first frame update
    void Start()
    {
        cameraTr = Camera.main.transform;
        anim = GetComponent<Animator>();
        playerTr = GetComponent<Transform>();

        cControll = GameObject.Find("Main Camera").GetComponent<CameraControll>();
        followTr = transform.Find("CameraFollow").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Vector2 inputDir = input.normalized;

        //CharacterRotation
        if(inputDir != Vector2.zero)
        {
            if (!isAiming)
            {
                float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + cameraTr.eulerAngles.y;
                transform.eulerAngles = Vector2.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);
            }
        }
        
        bool running = Input.GetKey(KeyCode.LeftShift);
        float speed = ((running) ? runSpeed : walkSpeed) * inputDir.magnitude;
        
        currentSpeed = Mathf.SmoothDamp(currentSpeed, speed, ref speedSmoothVelocity, speedSmoothTime);
        
        //이동처리
        //Aiming 상태가 아닐때에만 이동처리를 합니다.
        if(!isAiming)
        transform.Translate(transform.forward * currentSpeed * Time.deltaTime, Space.World);
        
        if(input.x == 0 && input.y == 0)
        {
            anim.SetBool(hashMoving, false);
        }
        else if (input.x != 0 || input.y != 0)
        {
            if(!isAiming)
            anim.SetBool(hashMoving, true);
        }
        
        if(Input.GetMouseButton(1))
        {
            isAiming = true;
            cControll._isZoomed = true;

            //Vector3.Lerp(transform.Find("CameraFollow").position, transform.Find("CameraFollow(Zoom)").position, 2.0f);
        }
        else if (Input.GetMouseButtonUp(1))
        {
            isAiming = false;
            cControll._isZoomed = false;

        }

        Aiming(); 
    }

    void Aiming()
    {
        if(isAiming)
        {
            Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            Vector2 inputDir = input.normalized;

            //Aiming Animation OutPut
            anim.SetFloat(hashV, Input.GetAxis("Vertical"));
            anim.SetFloat(hashH, Input.GetAxis("Horizontal"));

            float targetRotation = cameraTr.eulerAngles.y;
            transform.eulerAngles = Vector2.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);

            followTr.position = Vector3.Lerp(followTr.position,
               transform.Find("CameraFollow(Zoom)").position, 0.05f);
        }
        else if(!isAiming)
        {
            followTr.position = Vector3.Lerp(followTr.position,
              transform.Find("CameraFollow(noZoom)").position, 0.05f);
        }

        anim.SetBool(hashAiming, isAiming);
    }
}
